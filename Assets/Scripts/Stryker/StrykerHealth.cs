using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;
using UnityEngine.UI;

public class StrykerHealth : MonoBehaviour, ITargetHealth
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Text _textHp;
    public float Health { get; set; } = 100f;
    public bool IsArmored { get; set; } = true;

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
            Invoke("Destroy", 0.5f);
            Health = 0;
        }

        _hpBar.value = Health;
        _textHp.text = Health.ToString();
    }

    public void Destroy()
    {
        _eventBus.Invoke(new SetDie());
        Destroy(gameObject);
    }
}
