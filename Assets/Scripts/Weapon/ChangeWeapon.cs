using Game.Player;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _items;
    [SerializeField] private bool _startSettings = true;

    private bool _canTake = true;

    private EventBus _eventBus;
    private PlayerHealth _playerHealth;

    public int SyrgineAmount = 2;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuySyrgine>(AddSyrgine, 1);

        _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();

        _eventBus.Invoke(new UpdateSyrgine(SyrgineAmount));

        if (_startSettings)
        {
            Deactivate();
            _items[0].SetActive(true);
        }
    }

    private void Update()
    {
        if (_canTake)
        {
            CheckAndActivateItem(KeyCode.Alpha1, 0);
            CheckAndActivateItem(KeyCode.Alpha2, 1);
            CheckAndActivateItem(KeyCode.Alpha3, 2);
            CheckAndActivateItem(KeyCode.Alpha4, 3);
            CheckAndActivateItem(KeyCode.X, 4, true);
            CheckAndActivateItem(KeyCode.J, 5);
        }
    }

    private void CheckAndActivateItem(KeyCode key, int itemIndex, bool isSyrgine = false)
    {
        if (Input.GetKeyDown(key) && !_items[itemIndex].activeSelf)
        {
            if (!isSyrgine)
            {
                CancelInvoke("CanTake");
                _canTake = false;
                Invoke("CanTake", 0.8f);
                Deactivate();
                _items[itemIndex].SetActive(true);
            }

            if(isSyrgine)
            {
                if (_playerHealth.Health < 100f && SyrgineAmount > 0)
                {
                    CancelInvoke("CanTake");
                    _canTake = false;
                    Invoke("UseAfter", 1f);
                    Deactivate();
                    _items[itemIndex].SetActive(true);
                }
            }
        }
    }

    private void Deactivate()
    {
        foreach (GameObject item in _items)
        {
            item.SetActive(false);
        }
    }

    private void AddSyrgine(BuySyrgine syrgine)
    {
        SyrgineAmount += syrgine.Amount;
        _eventBus.Invoke(new UpdateSyrgine(SyrgineAmount));
    }

    private void UseAfter()
    {
        _canTake = true;
        Deactivate();
        _items[0].SetActive(true);
        _playerHealth.AddHealth(100f);
        SyrgineAmount--;
        _eventBus.Invoke(new UpdateSyrgine(SyrgineAmount));
    }

    private void CanTake()
    {
        _canTake = true;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuySyrgine>(AddSyrgine);
    }
}

