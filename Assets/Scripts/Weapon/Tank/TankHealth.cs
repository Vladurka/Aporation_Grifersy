using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class TankHealth : MonoBehaviour, ITargetHealth
{
    public float Health { get; set; } = 100f;
    public bool IsArmored { get; set; }

    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private GameObject _tower;
    [SerializeField] private Material _material;
    [SerializeField] private bool _isArmored = false;

    private MeshRenderer[] _renderers;

    private bool _isDead = false;

    private IVehicleShoot[] _tankShoot;
    private EventBus _eventBus;
    private Rigidbody _rb;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new AddObj(gameObject));
        _tankShoot = GetComponentsInChildren<IVehicleShoot>();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        IsArmored = _isArmored;
        _fire.Stop();
    }

    private void OnEnable()
    {
        if(!_isDead)
            _fire.Stop();
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
            Destroy();
    }

    public void Destroy()
    {
        if (!_isDead)
        {
            _eventBus.Invoke(new RemoveObj(gameObject));

            foreach (MeshRenderer renderer in _renderers)
            {
                Material[] materials = renderer.materials;

                Material[] newMaterials = new Material[materials.Length];

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = _material;
                }
                renderer.materials = newMaterials;
            }

            Instantiate(_explosion, _tower.transform.position, Quaternion.identity);

            float x = Random.Range(0f, 6f);
            float y = Random.Range(0f, 6f);
            float z = Random.Range(0f, 6f);

            if (_tower.TryGetComponent(out Rigidbody rb))
            {
                Vector3 dir = new Vector3(x, y, z);
                _rb.AddForce(dir * 2f, ForceMode.Impulse);
            }

            _fire.Play();

            foreach (IVehicleShoot vehicle in _tankShoot)
                vehicle.Stop();

            if(transform.TryGetComponent(out TankMove move))
                move.CanMove = false;

            _isDead = true;
        }
    }
}
