using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class TankHP : MonoBehaviour, ITankHealth
{
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private Transform _pos;
    [SerializeField] private Material _material;

    [SerializeField] private MeshRenderer[] _renderer;

    private bool _isDead = false;

    private EventBus _eventBus;
    private Rigidbody _rb;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new AddObj(gameObject));

        _fire.Stop();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(!_isDead)
            _fire.Stop();
    }

    public void Destroy()
    {
        if (!_isDead)
        {
            foreach (MeshRenderer renderer in _renderer)
            {
                Material[] materials = renderer.materials;

                Material[] newMaterials = new Material[materials.Length];

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = _material;
                }

                renderer.materials = newMaterials;
            }

            Instantiate(_explosion, _pos.position, Quaternion.identity);

            float x = Random.Range(0f, 6f);
            float y = Random.Range(0f, 6f);
            float z = Random.Range(0f, 6f);

            Vector3 dir = new Vector3(x, y, z);
            _rb.AddForce(dir * 2f, ForceMode.Impulse);
            _eventBus.Invoke(new DestroyTank());
            _eventBus.Invoke(new RemoveObj(gameObject));
            _isDead = true;
            _fire.Play();
        }
    }
}
