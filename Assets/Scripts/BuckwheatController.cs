using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class BuckwheatController : IService
{
    public int Amount;
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Invoke(new UpdateBuckhweat(Amount));
    }

    private void AddBuckweat(int amount)
    {
        Amount += amount;
    }

    public void SpendBuckweat(int amount)
    {
        Amount -= amount;
    }
}
