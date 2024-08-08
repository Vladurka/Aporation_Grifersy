using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using Game.Weapon;
using UnityEngine;

public class RpgBulletPool : MonoBehaviour, IService
{
    [SerializeField] private int _poolAmount = 10;
    [SerializeField] private bool _autoExpand = true;
    [SerializeField] private RocketLife _rocketLife;
    [SerializeField] private Transform _spawnPosition;

    private ObjectPool<RocketLife> _pool;
    private EventBus _eventBus;

    private void Start()
    {
       _eventBus = ServiceLocator.Current.Get<EventBus>();
       _eventBus.Subscribe<InstantRocket>(LaunchRocket, 1);

       _pool = new ObjectPool<RocketLife>(_rocketLife, _poolAmount, transform);
       _pool.AutoExpand = _autoExpand;
    }

    private void LaunchRocket(InstantRocket Rocket)
    {
        var rocket = _pool.GetFreeElement(_spawnPosition.position, _spawnPosition.rotation);

        rocket.transform.forward = Rocket.Dir.normalized;
        rocket.GetComponent<Rigidbody>().AddForce(Rocket.Dir.normalized * Rocket.Force, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<InstantRocket>(LaunchRocket);
    }
}
