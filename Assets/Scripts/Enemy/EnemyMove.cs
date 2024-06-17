using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMove : MonoBehaviour, IEnemyMove
    {
        [SerializeField] private float _damageObstacle = 1f;
        [SerializeField] private float _obstacleDetectionRange = 1f;

        private GameObject[] _points;
        private GameObject _mainCharacter;

        public bool IsDetected { get; set;} = false;

        private int _index = 0;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _points = GameObject.FindGameObjectsWithTag("Point");
            _agent = GetComponent<NavMeshAgent>();
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (IsDetected == false)
                Walk();

            if (IsDetected == true)
                EnemyDetected();

        }

        public void Walk()
        {
            _agent.SetDestination(_points[_index].transform.position);

            if (_agent.remainingDistance <= 0.01f && _index < _points.Length - 1)
            {
                _index++;
                return;
            }

            else if(_index >= _points.Length - 1)
            {
                _index = 0;
                return;
            }
        }

        public void EnemyDetected()
        {
            if (_mainCharacter != null)
            {
                _agent.SetDestination(_mainCharacter.transform.position);
            }
        }
    }
}
