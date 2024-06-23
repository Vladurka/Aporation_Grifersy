using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMove : MonoBehaviour, IEnemyMove
    {
        private Animator _animator;

        private GameObject[] _points;
        private GameObject _mainCharacter;

        private int _index;
        private NavMeshAgent _agent;

        public bool IsDetected { get; set; } = false;
        public bool IsDead { get; set; } = false;


        private void Start()
        {
            _points = GameObject.FindGameObjectsWithTag("Point");
            _agent = GetComponent<NavMeshAgent>();
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _index = Random.Range(0, _points.Length);
        }

        private void FixedUpdate()
        {
            if (IsDetected == false)
                Walk();

            if (IsDetected == true)
                EnemyDetected();

            if (IsDead == true)
                _agent.speed = 0f;
                
        }

        public void Walk()
        {
            _agent.speed = 1;
            _animator.SetBool("Run", false);
            _agent.SetDestination(_points[_index].transform.position);

            if (Vector3.Distance(transform.position, _points[_index].transform.position) <= 0.01f && _index < _points.Length - 1)
            {
                _index++;
                return;
            }

            if(_index >= _points.Length - 1)
            {
                _index = 0;
                return;
            }
        }

        public void EnemyDetected()
        {
            _agent.speed = 2.5f;
            _animator.SetBool("Run", true);
            if (_mainCharacter != null)
            {
                _agent.SetDestination(_mainCharacter.transform.position);
            }
        }
    }
}
