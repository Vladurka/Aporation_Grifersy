using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Shop : MonoBehaviour, IService
{
    private int _akBulletsAmount = 30;
    private int _rpgBulletsAmount = 5;

    private int _basePrice = 30;
    private int _baseUpgradeAmount = 0;
    private int _maxBaseUpgradeAmount = 0;

    private EventBus _eventBus;
    private CoinSystem _coinSystem;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
    }

    public void BuySkope(ScopesParametrs scope)
    {
        if (scope.IsBought == false && _coinSystem.Money >= scope.Price)
        {
            _coinSystem.SpendMoney(scope.Price);
            _eventBus.Invoke(new GetScope(scope.Level));
            scope.IsBought = true;
        }

        if(scope.IsBought == true)
            _eventBus.Invoke(new GetScope(scope.Level));
    }

    public void BuyBase()
    {
        if (_coinSystem.Money >= _basePrice && _baseUpgradeAmount < _maxBaseUpgradeAmount)
        {
            _eventBus.Invoke(new BuyBase());
            _coinSystem.SpendMoney(_basePrice);
            _basePrice += 25;
            _baseUpgradeAmount++;
        }
    }

    public void BuyAkBullets(int price)
    {
        if (_coinSystem.Money >= price)
        {
            _eventBus.Invoke(new BuyAkBullets(_akBulletsAmount));
            _coinSystem.SpendMoney(price);
        }
    }

    public void BuyRpgBullets(int price)
    {
        if (_coinSystem.Money >= price)
        {
            _eventBus.Invoke(new BuyRpgBullets(_rpgBulletsAmount));
            _coinSystem.SpendMoney(price);
        }
    }

    //public void BuyGrenade(int amount)
    //{
    //    _eventBus.Invoke(new BuyGrenades(amount));
    //}

    //public void BuyMine(int amount)
    //{
    //    _eventBus.Invoke(new BuyMine(amount));
    //}
}
