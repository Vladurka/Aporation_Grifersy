using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class GrenadeAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<Throw1>(GetThrow1, 1);
        _eventBus.Subscribe<Throw2>(GetThrow2, 1);
    }
    private void OnEnable()
    {
        _animator.SetTrigger("TakeGrenade");
    }

    private void GetThrow1(Throw1 throw1)
    {
        _animator.SetBool("Throw1", true);
    }

    private void GetThrow2(Throw2 throw2)
    { 
        _animator.SetBool("Throw1", false);
        _animator.SetTrigger("Throw2");
    }
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<Throw1>(GetThrow1);
        _eventBus.Unsubscribe<Throw2>(GetThrow2);
    }
}
