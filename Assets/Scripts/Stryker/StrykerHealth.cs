using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class StrykerHealth : MonoBehaviour, ITargetHealth
{
    public float Health { get; set; } = 100f;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
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

    public void Destroy()
    {
        _eventBus.Invoke(new SetDie());
        Destroy(gameObject);
    }
}
