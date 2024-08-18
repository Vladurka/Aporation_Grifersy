using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class AirdefenceHealth : MonoBehaviour, ITargetHealth
{
    public float Health { get; set; } = 100f;

    [SerializeField] private ParticleSystem[] _lowHpEffect;
    [SerializeField] private Material _destoyedMaterial;

    private MeshRenderer[] _meshRenderer;
    private EventBus _eventBus;

    private bool _gotDamage = false;
    private bool _isDead = false;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new AddObj(gameObject));

        _meshRenderer = GetComponentsInChildren<MeshRenderer>();

        foreach (ParticleSystem effect in _lowHpEffect)
            effect.Stop();
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (!_gotDamage)
        {
            foreach (ParticleSystem effect in _lowHpEffect)
                effect.Play();
            _gotDamage = true;
        }

        if (Health <= 0 && !_isDead)
            Destroy();
    }

    public void Destroy()
    {
        foreach (MeshRenderer mesh in _meshRenderer)
            mesh.material = _destoyedMaterial;

        if(transform.TryGetComponent(out IVehicleShoot vehicle))
            vehicle.Stop();

        _isDead = true;

        gameObject.tag = "Untagged";
        _eventBus.Invoke(new RemoveObj(gameObject));
    }
}
