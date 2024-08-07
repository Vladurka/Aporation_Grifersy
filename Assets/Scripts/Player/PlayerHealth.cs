using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour, IPlayerHealth, IService
    {
        [SerializeField] private Animator _bloodAnim;

        public float Health { get; set; } = 100f;

        private EventBus _eventBus;

        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Invoke(new UpdateHealth(Health));
        }

        public void Die()
        {
            _eventBus.Invoke(new SetDie());
            Destroy(gameObject);
        }

        public void GetDamage(float damage)
        {
            _bloodAnim.SetTrigger("Blood");

            Health -= damage;

            if (Health <= 0)
            {
                Invoke("Die", 0.5f);
                Health = 0;
            }

            _eventBus.Invoke(new UpdateHealth(Health));
        }
    }
}
