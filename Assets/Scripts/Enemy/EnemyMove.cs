using System.Collections;
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

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");
            _points = GameObject.FindGameObjectsWithTag("Point");
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
            if (_agent.remainingDistance <= 1f)
            {
                _index++;
                _agent.SetDestination(_points[_index].transform.position);
            }
        }

        public void EnemyDetected()
        {
            if (_mainCharacter != null)
            {
                _agent.SetDestination(_mainCharacter.transform.position);
                CheckForObstacles();
            }
        }

        private void CheckForObstacles()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _obstacleDetectionRange))
            {
                if (hit.collider.TryGetComponent(out IObstacleHealth obstacle))
                {
                    StartCoroutine(DestroyObstacle(obstacle));
                }
            }
        }

        private IEnumerator DestroyObstacle(IObstacleHealth obstacle)
        {
            _agent.isStopped = true;

            obstacle.GetDamage(_damageObstacle);

            if (obstacle != null)
            {
                StopAllCoroutines();
                _agent.isStopped = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
