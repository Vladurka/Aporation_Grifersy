using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
public class Instruments : MonoBehaviour, IInstrument, IInteractable
{
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void Get()
    {
        _eventBus.Invoke(new EndSignal());
        gameObject.SetActive(false);
    }
}
