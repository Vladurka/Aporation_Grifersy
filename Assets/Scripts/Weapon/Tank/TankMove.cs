using UnityEngine;
using UnityEngine.AI;

public class TankMove : MonoBehaviour
{
    private int _index;
    private GameObject[] _points;

    private NavMeshAgent Agent;
    private void Start()
    {
        _points = GameObject.FindGameObjectsWithTag("Point");
        Agent = GetComponent<NavMeshAgent>();
        _index = Random.Range(0, _points.Length);
    }

    private void Update()
    {
        Chill();
    }

    private void Chill()
    {
        Agent.speed = 1;
        Agent.SetDestination(_points[_index].transform.position);

        if (Agent.remainingDistance <= 2f)
        {
            _index = Random.Range(0, _points.Length);
            return;
        }
    }
}
