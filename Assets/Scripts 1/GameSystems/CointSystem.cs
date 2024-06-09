using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class CoinSystem : IService
{
    public int Money;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<MoneyAdd>(AddMoney, 1);
    }

    private void AddMoney(MoneyAdd money)
    {
        Money += money.Amount;
        Debug.Log(Money);
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        if (Money < 0)
            Debug.LogError("There is no money");
    }
}
