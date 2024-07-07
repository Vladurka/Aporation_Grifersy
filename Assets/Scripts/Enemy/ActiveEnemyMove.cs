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
            Agent = GetComponent<NavMeshAgent>();
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
                Agent.speed = 0f;

            if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= 20f)
                IsDetected = true;
        }

        public override void Chill()
        {
            Agent.speed = 1;
            _animator.SetBool("Run", false);
            Agent.SetDestination(_points[_index].transform.position);

            if (Agent.remainingDistance <= 2f)
            {
                _index = Random.Range(0, _points.Length);
                return;
            }
        }

        public override void EnemyDetected()
        {
            Agent.speed = 2.5f;
            _animator.SetBool("Run", true);
            if (_mainCharacter != null)
                Agent.SetDestination(_mainCharacter.transform.position);
        }
    }
}
