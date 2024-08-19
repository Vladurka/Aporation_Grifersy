using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BaseStates : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _states;

    public int BaseLevel = 0;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuyBase>(GetNewState, 1);

        foreach (GameObject state in _states)
            state.SetActive(false);

        if (BaseLevel >= 1 && BaseLevel - 1 < _states.Length)
        {
            for (int i = 0; i <= BaseLevel - 1; i++)
                _states[i].SetActive(true);
        }
    }

    private void GetNewState(BuyBase state)
    {
        if (BaseLevel < _states.Length)
        {
            BaseLevel++;
            if (_states[BaseLevel - 1])
                _states[BaseLevel - 1].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyBase>(GetNewState);
    }
}
