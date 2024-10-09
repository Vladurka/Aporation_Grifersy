using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class TaskBoard : MonoBehaviour, IOpenClose, IInteractable
{
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void Open()
    {
        if (PlayerPrefsSafe.HasKey(ConstSystem.PAPICH))
            _eventBus.Invoke(new OpenBoard());
    }

    public void Close()
    {
        _eventBus.Invoke(new CloseBoard());
    }
}
