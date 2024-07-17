using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ActiveEnemyMove : AbstractEnemy
    {
        private bool _isCalled = false;

        private int _index;

        private EventBus _eventBus;

        private void Start()
        {
            _points = GameObject.FindGameObjectsWithTag("Point");
            _agent = GetComponent<NavMeshAgent>();
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _index = Random.Range(0, _points.Length);
            _eventBus = ServiceLocator.Current.Get<EventBus>();
        }

        private void FixedUpdate()
        {
           if (!IsDetected)
               Chill();

            if (!_isMission5)
            {
                if (IsDetected)
                    EnemyDetected();
            }

            if (_isMission5)
            {
                if (IsDetected)
                {
                    SetSiren();
                    EnemyDetected();
                }
            }

            if (IsDead)
                _agent.speed = 0f;

            if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _range)
            {
                if (!IsDetected)
                    IsDetected = true;
            }
        }

        private void Chill()
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
                _agent.SetDestination(_mainCharacter.transform.position);
        }

        private void SetSiren()
        {
            if (!_isCalled)
            {
                _eventBus.Invoke(new SetSiren());
                _isCalled = true;
            }
        }
    }
}
