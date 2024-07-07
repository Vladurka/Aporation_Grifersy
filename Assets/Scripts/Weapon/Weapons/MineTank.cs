using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class MineTank : MonoBehaviour
{
    [SerializeField] private float _enemyDamage = 100f;
    [SerializeField] private float _range = 15f;
    [SerializeField] private float _callRange = 50f;
    [SerializeField] private ParticleSystem _effect;

    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        SetMine.explode += Explode;
    }

    private void Explode()
    {
        _eventBus.Invoke(new CheckList(transform.position, _callRange));
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider hit in hits)
        {
            if(hit.transform.TryGetComponent(out IEnemyHealth enemy))
            {
                enemy.GetDamage(_enemyDamage);
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
        SetMine.explode -= Explode;
    }

}
