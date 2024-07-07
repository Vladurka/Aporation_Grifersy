using Game.Enemy;
using Game.Player;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _range = 1f;
    [SerializeField] private float _damage = 20f;

    private bool _started = false;

    private Animator _animator;
    private GameObject _mainCharacter;
    private ActiveEnemyMove _move;

    private void Start()
    {
        _move = GetComponent<ActiveEnemyMove>();
        _animator = GetComponent<Animator>();
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) <= 3f)
        {
            if(!_started)
            {
                StartCoroutine(Attack());
                _move.Agent.Stop();
                _started = true;
            }
        }

        else if (_mainCharacter != null && Vector3.Distance(transform.position, _mainCharacter.transform.position) > 3f)
        {
            if (_started)
            {
                StopAllCoroutines();
                _move.Agent.Resume();
                _started = false;
            }
        }
    }

    private IEnumerator Attack()
    {
        _animator.SetTrigger("Attack");
        Collider[] hit = Physics.OverlapSphere(transform.position, _attackRange);
        foreach(Collider hits in hit) 
        {
            if (hits.transform.TryGetComponent(out IPlayerHealth health))
                health.GetDamage(_damage);
        }
        yield return new WaitForSeconds(1f);
    }
}
