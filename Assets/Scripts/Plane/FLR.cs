using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class FLR : MonoBehaviour
{
    private EventBus _eventBus;
    void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new ShootFLR(gameObject.transform));
        Destroy(gameObject, 10f);
    }
}
