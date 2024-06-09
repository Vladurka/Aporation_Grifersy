using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
public class RPGAnim : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<RpgShootAnim>(Shoot, 1);
    }
    private void OnEnable()
    {
        _animator.SetTrigger("TakeRPG");
    }

    private void Shoot(RpgShootAnim anim)
    {
        _animator.SetTrigger("ShootRPG");
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<RpgShootAnim>(Shoot);
    }
}
