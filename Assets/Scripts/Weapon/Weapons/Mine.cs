using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using Game.Player;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _enemyDamage = 10f;
    [SerializeField] private float _playerDamage = 7f;
    [SerializeField] private float _range = 15f;
    [SerializeField] private float _callRange = 50f;
    [SerializeField] private ParticleSystem _effect;

    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ExplodeMine>(Explode, 1);
    }

    private void Explode(ExplodeMine mine)
    {
        _eventBus.Invoke(new CheckList(transform.position, _callRange));
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider hit in hits)
        {
            if(hit.transform.TryGetComponent(out IEnemyHealth enemy))
            {
                enemy.GetDamage(_enemyDamage);
            }

            if (hit.transform.TryGetComponent(out IPlayerHealth player))
            {
                player.GetDamage(_playerDamage);
            }

            if(hit.transform.CompareTag("Tank"))
            {
                _eventBus.Invoke(new EndSignal());
            }
        }
        Instantiate(_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<ExplodeMine>(Explode);
    }

}
