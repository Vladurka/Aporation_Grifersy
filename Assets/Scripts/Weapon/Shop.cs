using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IService, IShop
{
    [Header("Bullets")]
    [SerializeField] private int _akBulletsAmount = 30;
    [SerializeField] private int _rpgBulletsAmount = 5;
    [SerializeField] private int _grenadesAmount = 1;
    [SerializeField] private int _syrgineAmount = 1;

    [Header("Base")]
    [SerializeField] private int _maxBaseUpgradeAmount = 4;
    [SerializeField] private int _basePrice = 500;
    [SerializeField] private Text _priceText;
    public int BaseUpgradeAmount = 0;

    [Header("Skopes")]
    [SerializeField] private Text _priceScope1;
    [SerializeField] private Text _priceScope2;
    [SerializeField] private ScopesParametrs _scopeInfo1;
    [SerializeField] private ScopesParametrs _scopeInfo2;

    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _cameraUI;

    private EventBus _eventBus;
    private CoinSystem _coinSystem;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _coinSystem = ServiceLocator.Current.Get<CoinSystem>();

        if(_scopeInfo1.Condition == 1)
            _priceScope1.enabled = false;

        if (_scopeInfo2.Condition == 1)
            _priceScope2.enabled = false;

        if (BaseUpgradeAmount >= _maxBaseUpgradeAmount)
            _priceText.enabled = false;

        _priceText.text = _basePrice.ToString();
    }

    public void BuySkope(ScopesParametrs scope)
    {
        if (scope.Condition == 0 && _coinSystem.Money >= scope.Price)
        {
            _coinSystem.SpendMoney(scope.Price);
            _eventBus.Invoke(new GetScope(scope.Level));
            scope.Condition = 1;
            _eventBus.Invoke(new SetScopeCondition());

            if (scope.Level == 1)
                _priceScope1.enabled = false;

            if (scope.Level == 2)
                _priceScope2.enabled = false;

            if(transform.TryGetComponent(out Button button))
                button.interactable = false;
        }

        if (scope.Condition == 1)
        {
            _eventBus.Invoke(new GetScope(scope.Level));
            _eventBus.Invoke(new SetScopeCondition());
        }
    }

    public void BuyBase()
    {
        if (_coinSystem.Money >= _basePrice && BaseUpgradeAmount < _maxBaseUpgradeAmount)
        {
            _eventBus.Invoke(new BuyBase());
            _coinSystem.SpendMoney(_basePrice);
            BaseUpgradeAmount++;
            _basePrice += 100;
            _priceText.text = _basePrice.ToString() + "$";
        }

        if(BaseUpgradeAmount >= _maxBaseUpgradeAmount)
        {
            if(transform.TryGetComponent(out Button button))
                button.interactable = false;    

            _priceText.enabled = false;
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
            _coinSystem.SpendMoney(price);
        }

    }

    public void BuySyrgine(int price)
    {
        if (_coinSystem.Money >= price)
        {
            _eventBus.Invoke(new BuySyrgine(_syrgineAmount));
            _coinSystem.SpendMoney(price);
        }
    }

    public void BuyDrone(int price)
    {
        if (_coinSystem.Money >= price)
        {
            _eventBus.Invoke(new BuyDrone());
            _coinSystem.SpendMoney(price);
        }
    }

    public void SetPanel(bool state)
    {
        _shopPanel.SetActive(state);
    }

    public void Open()
    {
        ConstSystem.IsBeasy = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        _mainCharacter.SetActive(false);
        _cameraUI.SetActive(true);
        _shopPanel.SetActive(true);
    }

    public void Close()
    {
        ConstSystem.IsBeasy = false;
        _shopPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _mainCharacter.SetActive(true);
        _cameraUI.SetActive(false);
    }

}
