using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Enemy
{
    public class EnemyHP : MonoBehaviour, IEnemyHealth
    {
        public bool IsDead { get; set; } = false;
        public int KillCost { get; set; } = 100;
        public float Health { get; set; } = 10f;

        private EventBus _eventBus;

        private Animator _animator;

        private void Start () 
        {
            _animator = GetComponent<Animator>();

            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _eventBus.Invoke(new AddObj(gameObject));
        }

        public void Die()
        {
            if (gameObject.TryGetComponent(out AbstractEnemy enemyMove))
            {
                IsDead = true;
                enemyMove.IsDead = true;
                _animator.SetTrigger("Die");
                Destroy(gameObject, 5f);
            }
        }

        public void GetDamage(float damage)
        {
            _animator.SetTrigger("GetDamage");

            Health -= damage;

            if (Health <= 0 && IsDead != true)
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
