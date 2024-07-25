using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class CrushDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _range  = 2f;
    [SerializeField] private string _tagToIgnore = "Missile";

    private EventBus _eventBus;

    private static bool _isDead  = false;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, _range))
        {
            if (!hit.collider.CompareTag(_tagToIgnore) && !_isDead)
            {
                _eventBus.Invoke(new SetDie());
            }
        }
    }
}
