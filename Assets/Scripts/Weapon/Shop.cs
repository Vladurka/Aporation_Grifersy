using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Shop : MonoBehaviour, IService
{
    [Header("Bullets-")]
    [SerializeField] private int _akBulletsAmount = 30;
    [SerializeField] private int _rpgBulletsAmount = 5;
    [SerializeField] private int _grenadesAmount = 1;
    [SerializeField] private int _minesAmount = 1;

    [Header("Base")]
    [SerializeField] private int _baseUpgradeAmount = 0;
    [SerializeField] private int _maxBaseUpgradeAmount = 4;

    [SerializeField] private GameObject _shopPanel;
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

    public void BuyBase(int price)
    {
        if (_coinSystem.Money >= price && _baseUpgradeAmount <= _maxBaseUpgradeAmount)
        {
            _eventBus.Invoke(new BuyBase());
            _coinSystem.SpendMoney(price);
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

    public void BuyGrenade(int price)
    {
        if (_coinSystem.Money >= price)
            _eventBus.Invoke(new BuyGrenades(_grenadesAmount));
    }

    public void BuyMine(int price)
    {
        if (_coinSystem.Money >= price)
            _eventBus.Invoke(new BuyMine(_minesAmount));
    }

    public void SetPanel(bool state)
    {
        _shopPanel.SetActive(state);
    }
}
