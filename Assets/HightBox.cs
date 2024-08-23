using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class HightBox : MonoBehaviour
{
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("F-14"))
            _eventBus.Invoke(new StartAirdefence());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("F-14"))
            _eventBus.Invoke(new StopAirdefence());
    }
}
