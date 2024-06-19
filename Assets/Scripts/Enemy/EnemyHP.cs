using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Enemy
{
    public class EnemyHP : MonoBehaviour, IEnemyHealth
    {
        public int KillCost { get; set; } = 100;
        public float Health { get; set; } = 10f;

        private EventBus _eventBus;

        private Animator _animator;

        private EnemyMove _enemyMove;

        private void Start () 
        {
            _enemyMove = GetComponent<EnemyMove>();

            _animator = GetComponent<Animator>();

            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _eventBus.Invoke(new AddObj(gameObject));
        }

        public void Die()
        {
            _enemyMove.IsDead = true;
            _animator.SetTrigger("Die");
            Destroy(gameObject, 5f);
        }

        public void GetDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                _eventBus.Invoke(new MoneyAdd(KillCost));
                Die();
            }
        }

        private void OnDestroy()
        {
            _eventBus.Invoke(new RemoveObj(gameObject));
        }
    }
}
