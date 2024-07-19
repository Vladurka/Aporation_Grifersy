using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BaseStates : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _states;

    public int BaseLevel = -1;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuyBase>(GetNewState, 1);

        foreach (GameObject state in _states)
        {
            state.SetActive(false);
        }

        if (BaseLevel >= 0)
        {
            for (int i = 0; i <= BaseLevel; i++)
            {
                _states[i].SetActive(true);
            }
        }
    }

    private void GetNewState(BuyBase state)
    {
        if (BaseLevel < _states.Length - 1)
        {
            BaseLevel++;
            _states[BaseLevel].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyBase>(GetNewState);
    }
}
