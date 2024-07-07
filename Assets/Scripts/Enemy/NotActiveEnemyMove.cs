using Game.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class NotActiveEnemyMove : AbstractEnemy
{
    private void Start()
    {
        _points = GameObject.FindGameObjectsWithTag("Point");
        Agent = GetComponent<NavMeshAgent>();
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
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
        _animator.SetBool("Run", false);
    }

    public override void EnemyDetected()
    {
        Agent.speed = 2.5f;
        _animator.SetBool("Run", true);
        if (_mainCharacter != null)
        {
            Agent.SetDestination(_mainCharacter.transform.position);
        }
    }
}
