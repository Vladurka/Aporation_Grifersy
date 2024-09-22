using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class HostageTrigger : MonoBehaviour
{
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHostageSaver saver))
        {
            if (saver.HostageSaved >= saver.HostageToSave)
                _eventBus.Invoke(new EndSignal());
        }
    }
}
