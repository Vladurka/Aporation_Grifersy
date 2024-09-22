using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class HostageSaver : MonoBehaviour, IHostageSaver
{
    public int HostageToSave { get; set; } = 3;
    public int HostageSaved { get; set; } = 0;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SaveHostage>(Save, 1);
    }

    public void Save(SaveHostage hostage)
    {
        HostageSaved++;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SaveHostage>(Save);
    }
}
