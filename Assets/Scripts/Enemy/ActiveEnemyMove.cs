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
            _agent = GetComponent<NavMeshAgent>();
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");

            if (!_mainCharacter)
                StartCoroutine(base.FindPlayer());

            _points = GameObject.FindGameObjectsWithTag("Point");
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

            if (_mainCharacter && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _range)
            {
                if (!IsDetected)
                    IsDetected = true;
            }
        }

        protected override void Chill()
        {
            _agent.speed = 1f;
            _animator.SetBool("Run", false);

            if (!_isGone)
            {
                _agent.SetDestination(_points[_index].transform.position);
                _isGone = true;
            }

            if (_agent.remainingDistance <= 2f)
            {
                _index = Random.Range(0, _points.Length);
                _isGone = false;
            }
        }

        protected override void EnemyDetected()
        {
            if (_mainCharacter && _mainCharacter.activeSelf)
            {
                _agent.speed = 2.5f;
                _animator.SetBool("Run", true);
                _agent.SetDestination(_mainCharacter.transform.position);
            }

            else
            {
                if (IsDetected)
                    IsDetected = false;
            }
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
