using Game.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class NotActiveEnemyMove : AbstractEnemy
{
    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");

        if (!_mainCharacter)
            StartCoroutine(base.FindPlayer());

        _points = GameObject.FindGameObjectsWithTag("Point");

        _agent = GetComponent<NavMeshAgent>();
        int priority = Random.Range(0, 50);
        _agent.avoidancePriority = priority;

        _animator = GetComponent<Animator>();
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

    protected override void Chill()
    {
        _animator.SetBool("Run", false);
    }

    protected override void EnemyDetected()
    {
        _agent.speed = 2.5f;
        _animator.SetBool("Run", true);
        if (_mainCharacter != null)
            _agent.SetDestination(_mainCharacter.transform.position);
    }
}
