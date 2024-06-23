using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class HelicopterHealth : MonoBehaviour, IHelicopterHealth
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
    }
}
