using Game.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMove : AbstractEnemy
{
    private RaycastHit _hit;
    private SoldierShoot _soldierShoot;  
    public bool _isStarted;
    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");

        if (_mainCharacter.Equals(null))
            StartCoroutine(base.FindPlayer());

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _soldierShoot = GetComponent<SoldierShoot>();
    }
    private void FixedUpdate()
    {
        if (IsDead)
            _agent.speed = 0f;

        if (IsDetected && !IsDead)
            EnemyDetected();

        if (!_mainCharacter.Equals(null) && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _range)
        {
            if (!IsDetected)
                IsDetected = true;
        }
    }

    protected override void EnemyDetected()
    {
        if (Physics.SphereCast(transform.position, 1f, transform.forward, out _hit, 1000f))
        {
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
            if (_hit.collider.CompareTag("Player"))
            {
                _agent.isStopped = true;
                transform.LookAt(_mainCharacter.transform.position);

                if (!_isStarted)
                {
                    _soldierShoot.StartCoroutine(_soldierShoot.Shoot());
                    _isStarted = true;
                }
                _animator.SetBool("Run", false);
                _animator.SetBool("Ready", true);

                return;
            }

            else
            {
                _agent.isStopped = false;

                if (_isStarted)
                {
                    _soldierShoot.StopAllCoroutines();
                    _isStarted = false;
                }

                if(!_mainCharacter.Equals(null))
                    _agent.SetDestination(_mainCharacter.transform.position);

                _animator.SetBool("Run", true);
                _animator.SetBool("Ready", false);

                return;
            }
        }
    }
}
