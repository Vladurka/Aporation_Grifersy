using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BoxWithBullets : MonoBehaviour, IBox
{
    private bool _isOpen = false;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void Open()
    {
       if (!_isOpen)
       {
            _eventBus.Invoke(new BuyAkBullets(100));
            _eventBus.Invoke(new BuyRpgBullets(5));
            _eventBus.Invoke(new EndSignal());
            _isOpen = true;
       }
    }
}
