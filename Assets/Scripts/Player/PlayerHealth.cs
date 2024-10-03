using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour, IPlayerHealth, IService
    {
        [SerializeField] private Animator _bloodAnim;

        [SerializeField] private AudioClip[] _hurtSound;
        [SerializeField] private AudioClip _dieSound;

        public float Health { get; set; } = 100f;

        private EventBus _eventBus;
        private AudioSource _audioSource;

        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Invoke(new UpdateHealth(Health));

            _audioSource = GetComponent<AudioSource>();
            PlayerPrefs.SetInt(ConstSystem.STARTED_TO_PLAY, 1);
        }

        public void AddHealth(float amount)
        {
            Health += amount;

            if (Health > 100f)
                Health = 100f;

            _eventBus.Invoke(new UpdateHealth(Health));
        }

        public void GetDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Invoke("Die", 0.5f);
                Health = 0;
                _audioSource.PlayOneShot(_dieSound);
            }

            else
            {
                int index = Random.Range(0, _hurtSound.Length);
                _audioSource.PlayOneShot(_hurtSound[index]);
            }

            _eventBus.Invoke(new UpdateHealth(Health));
            _bloodAnim.SetTrigger("Blood");
        }

        public void Die()
        {
            _eventBus.Invoke(new SetDie());
            Destroy(gameObject);
        }
    }
}
