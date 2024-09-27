using Game.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMove : AbstractEnemy
{
    private RaycastHit _hit;
    private SoldierShoot _soldierShoot;  
    private bool _isStarted;
    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag(_tag);
        _animator = GetComponent<Animator>();

        if (_mainCharacter == null)
            StartCoroutine(base.FindPlayer());

        _agent = GetComponent<NavMeshAgent>();
        int priority = Random.Range(0, 50);
        _agent.avoidancePriority = priority;

        _soldierShoot = GetComponent<SoldierShoot>();
    }
    private void FixedUpdate()
    {
        if (IsDead)
            _agent.speed = 0f;

        if (IsDetected && !IsDead)
            EnemyDetected();

        if (!IsDetected)
        {
            _animator.SetBool("Run", false);
            _animator.SetBool("Ready", false);
            _agent.isStopped = true;
        }

        if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _range)
        {
            if (!IsDetected && _mainCharacter.activeSelf)
                IsDetected = true;
        }
    }

    protected override void EnemyDetected()
    {
        if (_mainCharacter != null && _mainCharacter.activeSelf)
        {
            if (Physics.SphereCast(transform.position, 1f, transform.forward, out _hit, 200f))
            {
                if (_hit.collider.CompareTag(_tag))
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

                    _agent.SetDestination(_mainCharacter.transform.position);

                    _animator.SetBool("Run", true);
                    _animator.SetBool("Ready", false);

                    return;
                }
            }
        }

        else
        {
            if(IsDetected)
                IsDetected = false;
        }
    }
}
