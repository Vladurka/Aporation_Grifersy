using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour, IPlayerHealth, IService
    {
        public float Health { get; set; } = 100f;

        private EventBus _eventBus;

        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Invoke(new UpdateHealth(Health));
        }

        public void Die()
        {
            Cursor.lockState = CursorLockMode.None;
            _eventBus.Invoke(new SetDie());
            Destroy(gameObject);
        }

        public void GetDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Die();
                Health = 0;
            }

            _eventBus.Invoke(new UpdateHealth(Health));
        }
    }
}
