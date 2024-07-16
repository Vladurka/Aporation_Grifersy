using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] _items;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<UseBuilder>(UseBuilder, 1);

        Deactivate();
        _items[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !_items[0].activeSelf)
        {
            Deactivate();
            _items[0].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !_items[1].activeSelf)
        {
            Deactivate();
            _items[1].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !_items[2].activeSelf)
        {
            Deactivate();
            _items[2].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && !_items[3].activeSelf)
        {
            Deactivate();
            _items[3].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && !_items[5].activeSelf)
        {
            if (_items[5] != null)
            {
                Deactivate();
                _items[5].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.V) && !_items[4].activeSelf)
        {
            Deactivate();
            _items[4].SetActive(true);
        }


    }

    private void Deactivate()
    {
        foreach (GameObject item in _items)
        {
            item.SetActive(false);
        }
    }

    private void UseBuilder(UseBuilder builder)
    {
        Deactivate();
        _items[5].SetActive(true);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<UseBuilder>(UseBuilder);
    }
}

