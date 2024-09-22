using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Hostage : MonoBehaviour, IHostage
{
    [SerializeField] private Transform _target;

    private bool _checkForHelicopter = false;

    private NavMeshAgent _agent;
    private Animator _animator;
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        Stay();
    }

    private void Update()
    {
        if(_checkForHelicopter)
        {
            _animator.SetBool("Run", true);
            _agent.SetDestination(_target.position);

            if (Vector3.Distance(transform.position, _target.position) <= 2f)
            {
                _eventBus.Invoke(new SaveHostage());
                Destroy(gameObject);
            }
        }
    }

    public void Save()
    {
        _checkForHelicopter = true;
    }

    public void Stay()
    {
        _animator.SetBool("Run", false);
    }
}
