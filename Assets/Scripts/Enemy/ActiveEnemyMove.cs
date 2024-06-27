using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ActiveEnemyMove : AbstractEnemy
    {
        private int _index;
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
            if (!IsDetected)
                Chill();

            if (IsDetected)
                EnemyDetected();

            if (IsDead)
                _agent.speed = 0f;

            if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= 20f)
                IsDetected = true;
        }

        public override void Chill()
        {
            _agent.speed = 1;
            _animator.SetBool("Run", false);
            _agent.SetDestination(_points[_index].transform.position);

            if (_agent.remainingDistance <= 2f)
            {
                _index = Random.Range(0, _points.Length);
                return;
            }
        }

        public override void EnemyDetected()
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
