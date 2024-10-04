using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private NavMeshAgent _agent;  
    [SerializeField] private float _stoppingDistance = 4f;

    [HideInInspector] public GameObject Target;
    [HideInInspector] public bool CanMove;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.stoppingDistance = _stoppingDistance;
        _agent.updateRotation = true;  
        _agent.updatePosition = true;  
        _agent.speed = 5f;  
    }

    private void FixedUpdate()
    {
        if (Target != null && CanMove)
        {
            _agent.SetDestination(Target.transform.position);

            if (_agent.remainingDistance <= _agent.stoppingDistance)
                StopTank();

            else
                ContinueTank();

        }

        else
            StopTank();
    }

    private void StopTank()
    {
        if(!_agent.isStopped)
            _agent.isStopped = true;
    }

    private void ContinueTank()
    {
        if (_agent.isStopped)
            _agent.isStopped = false;
    }
}
