using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private NavMeshAgent _agent;

    /*[HideInInspector]*/ public bool CanMove = true;
    [HideInInspector] public GameObject Target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!Target.Equals(null) && CanMove && !_agent.isStopped)
        {
            _agent.SetDestination(Target.transform.position);
            _agent.isStopped = false;
        }

        else if(_agent.isStopped && !CanMove)
            _agent.isStopped = true;
    }    
}
