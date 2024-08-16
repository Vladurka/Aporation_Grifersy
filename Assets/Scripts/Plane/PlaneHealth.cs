using System.Collections;
using UnityEngine;

public class PlaneHealth : MonoBehaviour, ITargetHealth
{
    public float Health { get; set; } = 100f;

    [SerializeField] private ParticleSystem[] _lowHpEffect;
    [SerializeField] private Material _destoyedMaterial;

    private MeshRenderer[] _meshRenderer;

    private bool _gotDamage = false;
    private bool _isDead = false;

    private Rigidbody _rb;

    private PlaneScopeCamera _scopeCamera;
    private PlaneController _controller;

    public void Init()
    {
        _controller = ServiceLocator.Current.Get<PlaneController>();
        _scopeCamera = GetComponentInChildren<PlaneScopeCamera>();
        _rb = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        if (!_gotDamage)
        {
            foreach (ParticleSystem effect in _lowHpEffect)
                effect.Stop();
        }
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

        if (Health <= 0)
            Destroy();
    }

    public void Destroy()
    {
        if (!_isDead)
        {
            _rb.useGravity = true;
            _rb.mass = 2f;
            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10f;
            _rb.AddTorque(randomTorque, ForceMode.Impulse);

            if (transform.TryGetComponent(out PlaneController planeController))
            {
                planeController.Force = 0;
                planeController.CanFly = false;
            }

            _scopeCamera.enabled = false;

            foreach (MeshRenderer mesh in _meshRenderer)
                mesh.material = _destoyedMaterial;

            StartCoroutine(Stopping());
            _isDead = true;
        }
    }

    private IEnumerator Stopping()
    {
        _controller.FlySpeed -= 3;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Stopping());
    }

}
