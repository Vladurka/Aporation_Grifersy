using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private NavMeshAgent _agent;

    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public GameObject Target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (Target != null && CanMove)
        {
            _agent.SetDestination(Target.transform.position);
            _agent.speed = 3.5f;
        }

        else
            _agent.speed = 0f;
    }    
}
