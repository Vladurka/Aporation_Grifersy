using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Enemy
{
    public class EnemyHP : MonoBehaviour, IEnemyHealth
    {
        private bool _isAdded = false;

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

            _isAdded = true;
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
            if (HasTrigger(_animator, "GetDamage"))
                _animator.SetTrigger("GetDamage");

            Health -= damage;

            if (Health <= 0 && IsDead != true)
            {
                _eventBus.Invoke(new MoneyAdd(KillCost));
                Die();
            }
        }

        private bool HasTrigger(Animator animator, string triggerName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName)
                    return true;
            }
            return false;
        }

        private void OnDestroy()
        {
            if(_isAdded)
                _eventBus.Invoke(new RemoveObj(gameObject));
        }
    }
}
