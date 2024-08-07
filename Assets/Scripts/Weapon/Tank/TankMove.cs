using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    [SerializeField] private string _1targetTag;
    [SerializeField] private string _2targetTag;

    private GameObject _target;
    private NavMeshAgent _agent;
    
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(_1targetTag);
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        _agent.SetDestination(_target.transform.position);
    }

    
}
