using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class CrushDetector : MonoBehaviour
{
    [SerializeField] private float _range  = 5f;
    [SerializeField] private string _tagToIgnore = "Missile";
    [SerializeField] private float _criticalAngle = 10f;

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
            float angle = Vector3.Angle(hit.normal, transform.position);

            if (!hit.collider.CompareTag(_tagToIgnore) && !_isDead)
            {
                if (_criticalAngle <= angle)
                    _eventBus.Invoke(new SetDie());
            }
        }
    }
}
