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
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _soldierShoot = GetComponent<SoldierShoot>();
    }
    private void FixedUpdate()
    {
        if (IsDetected)
            EnemyDetected();

        if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _range)
        {
            if (!IsDetected)
                IsDetected = true;
        }
    }

    public override void EnemyDetected()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                if (!_isStarted)
                {
                    _soldierShoot.StartCoroutine(_soldierShoot.Shoot());
                    _isStarted = true;
                }
                _animator.SetBool("Run", false);
            }

            else
            {
                if (_isStarted)
                {
                    _soldierShoot.StopAllCoroutines();
                    _isStarted = false;
                }

                _agent.SetDestination(_mainCharacter.transform.position);
                _animator.SetBool("Run", true);
            }
        }

    }
}
