using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BaseStates : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _states;

    private int _currentState = -1;

    public int BaseLevel = 0;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuyBase>(GetNewState, 1);

        foreach (GameObject state in _states)
        {
            state.SetActive(false);
        }
    }

    private void GetNewState(BuyBase state)
    {
        if (_currentState < _states.Length - 1)
        {
            _currentState++;
            _states[_currentState].SetActive(true);
            BaseLevel++;
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyBase>(GetNewState);
    }
}
