using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class CoinSystem : IService
{
    public int Money = 10000;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Invoke(new UpdateMoney(Money));
        _eventBus.Subscribe<MoneyAdd>(AddMoney, 1);
    }

    private void AddMoney(MoneyAdd money)
    {
        Money += money.Amount;
        _eventBus.Invoke(new UpdateMoney(Money));
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        _eventBus.Invoke(new UpdateMoney(Money));;
    }
}
