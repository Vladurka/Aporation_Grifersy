using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _callRange = 70f;
    [SerializeField] private float _explosionRadius = 6f;
    [SerializeField] private float _explosionDelay = 4f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _explosionEffect;

    [SerializeField] private AudioClip _explosionSound;

    private bool _isExploded = false;

    private MeshRenderer _renderer;
    private AudioSource _audioSource;
    private EventBus _eventBus;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        _explosionDelay -= Time.deltaTime;

        if(_explosionDelay <= 0f)
            Explode();
    }

    public void Throw(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }

    private void Explode()
    {
        if (!_isExploded)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (Collider hit in hits)
            {
                if (hit.transform.TryGetComponent(out IEnemyHealth enemy))
                {
                    enemy.GetDamage(_damage);
                }
            }
            _eventBus.Invoke(new CheckList(transform.position, _callRange));
            _audioSource.PlayOneShot(_explosionSound);
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            _renderer.enabled = false;
            Destroy(gameObject, 2f);
            _isExploded = true;
        }
    }
}
