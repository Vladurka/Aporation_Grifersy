using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 50f;
    [SerializeField] private float _stoppingDistance = 1.5f;  
    [SerializeField] private float _angleTolerance = 1f; 

    [HideInInspector] public bool CanMove = true;
    [HideInInspector] public GameObject Target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        int priority = Random.Range(0, 50);
        _agent.avoidancePriority = priority;

        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.nextPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (Target != null && CanMove)
        {
            _agent.nextPosition = transform.position;
            _agent.SetDestination(Target.transform.position);

            Vector3 directionToTarget = (_agent.steeringTarget - transform.position).normalized;
            Vector3 forward = transform.forward;
            float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);

            if (Mathf.Abs(angle) > _angleTolerance)
            {
                float turnDirection = Mathf.Sign(angle); 
                transform.Rotate(Vector3.up, turnDirection * _turnSpeed * Time.deltaTime);
            }
            else
            {
                float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

                if (distanceToTarget > _stoppingDistance)
                    transform.position += transform.forward * _moveSpeed * Time.deltaTime;

                else
                    _agent.ResetPath();  
            }
        }
    }
}
