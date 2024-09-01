using DG.Tweening;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ShakeCamera>(Shake);
    }

    private void Shake(ShakeCamera camera)
    {
        transform
            .DOShakePosition(camera.Duration, camera.Force, 2, 20f, false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce)
            .SetLink(transform.gameObject);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<ShakeCamera>(Shake);
    }

}
