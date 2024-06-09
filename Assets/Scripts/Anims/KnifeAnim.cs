using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class KnifeAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<KnifeShootAnim>(Shoot, 1);
    }

    private void OnEnable()
    {
        _animator.SetTrigger("TakeKnife");
    }

    private void Shoot(KnifeShootAnim anim)
    {
        _animator.SetTrigger("ShootKnife");
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<KnifeShootAnim>(Shoot);
    }
}
