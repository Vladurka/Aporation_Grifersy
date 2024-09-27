using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private float _turnSpeed = 50f;

    /*[HideInInspector]*/ public bool CanMove = true;
    [HideInInspector] public GameObject Target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        int priority = Random.Range(0, 50);
        _agent.avoidancePriority = priority;

        _agent.updatePosition = false;  
        _agent.updateRotation = false;  
    }

    private void Update()
    {
        if (Target != null && CanMove)
        {
            _agent.SetDestination(Target.transform.position);

            Vector3 directionToTarget = (_agent.steeringTarget - transform.position).normalized;

            Vector3 forward = transform.forward;

            float angle = Vector3.SignedAngle(forward, directionToTarget, Vector3.up);

            if (Mathf.Abs(angle) > 1f)
            {
                float turnDirection = Mathf.Sign(angle); 
                transform.Rotate(Vector3.up, turnDirection * _turnSpeed * Time.deltaTime);
            }

            else
                transform.position += transform.forward * _moveSpeed * Time.deltaTime;

            _agent.nextPosition = transform.position;
        }
    }
}