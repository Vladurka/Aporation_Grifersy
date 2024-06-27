using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class AKAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<AkShootAnim>(Shoot, 1);
        _eventBus.Subscribe<AkReloadAnim>(Reload, 1);
    }

    private void OnEnable()
    {
        ResetTriggers();
        _animator.Play("Take_AK");
    }

    private void Reload(AkReloadAnim anim)
    {
        ResetTriggers();
        _animator.SetTrigger("ReloadAK");
    }

    private void Shoot(AkShootAnim anim)
    {
        ResetTriggers();
        _animator.SetTrigger("ShootAK");
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<AkShootAnim>(Shoot);
        _eventBus.Unsubscribe<AkReloadAnim>(Reload);
    }

    private void ResetTriggers()
    {
        _animator.ResetTrigger("TakeAK");
        _animator.ResetTrigger("ReloadAK");
        _animator.ResetTrigger("ShootAK");
    }
}
