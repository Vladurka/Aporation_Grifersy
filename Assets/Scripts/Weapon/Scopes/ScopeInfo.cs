using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class ScopeInfo : MonoBehaviour
{
    [SerializeField] private ScopesParametrs _scopeParametrs;

    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetScopeCondition>(SetCondition, 1);

        _scopeParametrs.Condition = PlayerPrefsSafe.GetInt(_scopeParametrs.Key);
    }
    private void SetCondition(SetScopeCondition scope)
    {
        PlayerPrefsSafe.SetInt(_scopeParametrs.Key, _scopeParametrs.Condition);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetScopeCondition>(SetCondition);
    }
}
