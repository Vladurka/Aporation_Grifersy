using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class TankHP : MonoBehaviour, ITankHealth
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Transform _pos;

    private bool _isDead = false;

    private EventBus _eventBus;
    private Rigidbody _rb;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _rb = GetComponent<Rigidbody>();

        _rb.useGravity = false;
    }
    public void Destroy()
    {
        if (!_isDead)
        {
            _rb.useGravity = true; 

            Instantiate(_effect, _pos.position, Quaternion.identity);

            float x = Random.Range(0f, 6f);
            float y = Random.Range(0f, 6f);
            float z = Random.Range(0f, 6f);

            Vector3 dir = new Vector3(x, y, z);
            _rb.AddForce(dir * 2f, ForceMode.Impulse);
            _eventBus.Invoke(new DestroyTank());
            _eventBus.Invoke(new EndSignal());
            _isDead = true;
        }
    }
}
