using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Shop : MonoBehaviour, IService, IShop
{
    [Header("Bullets-")]
    [SerializeField] private int _akBulletsAmount = 30;
    [SerializeField] private int _rpgBulletsAmount = 5;
    [SerializeField] private int _grenadesAmount = 1;
    [SerializeField] private int _minesAmount = 1;

    [Header("Base")]
    [SerializeField] private int _baseUpgradeAmount = 0;
    [SerializeField] private int _maxBaseUpgradeAmount = 4;
    [SerializeField] private int _basePrice = 500;

    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _cameraUI;

    private EventBus _eventBus;
    private CoinSystem _coinSystem;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
    }

    public void BuySkope(ScopesParametrs scope)
    {
        if (scope.Condition == 0 && _coinSystem.Money >= scope.Price)
        {
            _coinSystem.SpendMoney(scope.Price);
            _eventBus.Invoke(new GetScope(scope.Level));
            scope.Condition = 1;
            _eventBus.Invoke(new SetScopeCondition());
        }

        if (scope.Condition == 1)
        {
            _eventBus.Invoke(new GetScope(scope.Level));
            _eventBus.Invoke(new SetScopeCondition());
        }
    }

    public void BuyBase()
    {
        if (_coinSystem.Money >= _basePrice && _baseUpgradeAmount <= _maxBaseUpgradeAmount)
        {
            _eventBus.Invoke(new BuyBase());
            _coinSystem.SpendMoney(_basePrice);
            _baseUpgradeAmount++;
            _basePrice += 500;
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
        {
            _eventBus.Invoke(new BuyGrenades(_grenadesAmount));
        }

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

    public void Open()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        _mainCharacter.SetActive(false);
        _cameraUI.SetActive(true);
        _shopPanel.SetActive(true);
    }

    public void Close()
    {
        _shopPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _mainCharacter.SetActive(true);
        _cameraUI.SetActive(false);
    }

}
