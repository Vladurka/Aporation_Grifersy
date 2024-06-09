using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class ScopeLevels : MonoBehaviour, IService
{
    [SerializeField] private GameObject[] _scopes;
    public int ScopeLevel = 0;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<GetScope>(GetSkope, 1);

        foreach (GameObject scopes in _scopes)
        {
            scopes.SetActive(false);
            _scopes[ScopeLevel].SetActive(true);
        }
    }

    private void GetSkope(GetScope scope)
    {
        ScopeLevel = scope.Level;  
         foreach (GameObject scopes in _scopes)
        {
            scopes.SetActive(false);
            _scopes[ScopeLevel].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GetScope>(GetSkope);
    }
}
