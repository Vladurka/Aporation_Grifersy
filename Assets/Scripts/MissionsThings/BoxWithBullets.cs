using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BoxWithBullets : MonoBehaviour, IBox
{
    public bool IsOpen { get; set; } = false;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void Open()
    {
       if (!IsOpen)
       {
            _eventBus.Invoke(new BuyAkBullets(100));
            _eventBus.Invoke(new BuyRpgBullets(5));
            IsOpen = true;
       }
    }
}
