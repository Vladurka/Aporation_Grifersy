using UnityEngine;
using UnityEngine.AI;

public class PapichMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] _points;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Transform _board;

    private bool _isStarted;
    private Animator _animator;
    private NavMeshAgent _agent;

    private int _index;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _index = Random.Range(0, _points.Length);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _mainCharacter.transform.position) <= 10f)
        {
            _isStarted = true;
            Talk();
        }

        if (!_isStarted)
            Chill();
    }

    private void Talk()
    {
        if(_isStarted)
        {
            if(!_audioSource.isPlaying)
                _audioSource.Play();

            //_animator.SetBool("Talk", true);
            _agent.speed = 0;
            _animator.SetBool("Walk", false);
            transform.LookAt(_mainCharacter.transform.position);
        }
    }

    private void Walk()
    {

    }

    private void Chill()
    {
        _agent.speed = 1;
        _animator.SetBool("Walk", true);
        _agent.SetDestination(_points[_index].transform.position);

        if (_agent.remainingDistance <= 2f)
        {
            _index = Random.Range(0, _points.Length);
            return;
        }
    }

}