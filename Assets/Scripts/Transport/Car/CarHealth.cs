using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class CarHealth : MonoBehaviour, ITransportHealth
{
    public float Health { get; set; } = 100f;

    private AudioSource[] _audioSource;
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _audioSource = GetComponentsInChildren<AudioSource>();
    }

    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        _eventBus.Invoke(new SetDie());

        if (ConstSystem.InTransport)
        {
            foreach (AudioSource sourse in _audioSource)
                sourse.Stop();

            ConstSystem.CanSave = false;
            Destroy(gameObject);
        }
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Invoke("Die", 0.5f);
            Health = 0;
        }
    }
}
