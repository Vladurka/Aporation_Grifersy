using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class HelicopterHealth : MonoBehaviour, ITransportHealth
{
    public float Health { get; set; } = 100f;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        _eventBus.Invoke(new SetDie());
        if (ConstSystem.InTransport)
        {
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
