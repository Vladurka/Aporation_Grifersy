using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.AI;

public class PapichMovement : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _points;
    [SerializeField] private GameObject _mainCharacter;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _clip1;
    [SerializeField] private AudioClip _clip2;

    [SerializeField] private Transform _board;
    [SerializeField] private Transform _hidePoint;

    public enum State { Patrol, Talk1, Talk2, GoToPoint, TalkAtPoint, Hide }

    public State CurrentState;

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    private EventBus _eventBus;

    private int _index;

    private bool _isStarted = false;
    private bool _played1 = false;
    private bool _played2 = false;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        if (PlayerPrefsSafe.HasKey(ConstSystem.PAPICH))
            _isStarted = true;

        if(!PlayerPrefsSafe.HasKey(ConstSystem.PAPICH))
            _isStarted = false;

        CurrentState = State.Patrol;
        _index = Random.Range(0, _points.Length);
    }

    private void FixedUpdate()
    {
        if (!_isStarted)
        {
            switch (CurrentState)
            {
                case State.Patrol:
                    Patrol();
                    break;
                case State.Talk1:
                    Talk1();
                    break;
                case State.Talk2:
                    Talk2(_clip2);
                    break;
                case State.GoToPoint:
                    Walk();
                    break;
            }
        }

        if (_isStarted)
        {
            switch (CurrentState)
            {
                case State.Patrol:
                    Patrol();
                    break;

                case State.Hide:
                    Hide();
                    break;
            }
        }
    }

    private void Walk()
    {
        _agent.speed = 1;
        _agent.SetDestination(_board.position);
        _animator.SetBool("Walk", true);
        if (_agent.remainingDistance <= 0.2f)
            CurrentState = State.Talk2;
    }

    private void Patrol()
    {
        _agent.speed = 1;
        _animator.SetBool("Walk", true);
        _agent.SetDestination(_points[_index].transform.position);

        if (_agent.remainingDistance <= 2f)
        {
            _index = Random.Range(0, _points.Length);
            return;
        }

        if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= 10f)
            CurrentState = State.Talk1;
    }

    private void Talk1()
    {
        if (!_audioSource.isPlaying && !_played1)
        {
            _eventBus.Invoke(new NextTip());
            _audioSource.PlayOneShot(_clip1);
            _played1 = true;
        }

        if (!_audioSource.isPlaying && _played1)
        {
            _audioSource.Stop();
            CurrentState = State.GoToPoint;
        }

        _agent.speed = 0;
        _animator.SetBool("Walk", false);
        transform.LookAt(_mainCharacter.transform.position);
    }

    private void Talk2(AudioClip clip)
    {
        if (!_audioSource.isPlaying && !_played2)
        {
            _audioSource.PlayOneShot(clip);
            _played2 = true;
        }

        if (!_audioSource.isPlaying && _played2)
        {
            _audioSource.Stop();
            PlayerPrefsSafe.SetInt(ConstSystem.PAPICH, 1);
            _isStarted = true;
            CurrentState = State.Patrol;
        }

        _agent.speed = 0;
        _animator.SetBool("Walk", false);
        transform.LookAt(_mainCharacter.transform.position);
    }

    private void Hide()
    {
        _agent.speed = 1;
        _agent.SetDestination(_hidePoint.position);

        if (_agent.remainingDistance <= 1f)
            _agent.speed = 0f;
    }
}