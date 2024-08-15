using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 600f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _lifeTime = 10f;
    [SerializeField] private bool _lfrResistent = false;


    [Header("Effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _flyEffect;

    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private AudioClip _destroySound;

    [HideInInspector] public Transform Target;

    private bool _isExploded = false;

    private EventBus _eventBus;

    private AudioSource _audioSource;
    private MeshRenderer[] _meshRenderer;


    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ShootFLR>(FLR, 1);
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        _audioSource.PlayOneShot(_bulletSound);

        StartCoroutine(Effect());
        Invoke("BulletDestroy", _lifeTime);
    }

    void Update()
    {
        if (Target != null)
        {
            Vector3 dir = Target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            float distanceThisFrame = _speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
                BulletDestroy();

            else
                transform.Translate(Vector3.forward * distanceThisFrame);

            transform.LookAt(Target.position);
        }
    }

    private IEnumerator Effect()
    {
        Instantiate(_flyEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Effect());
    }

    private void BulletDestroy()
    {
        if (!_isExploded)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out ITargetHealth target))
                    target.GetDamage(_damage);
            }
            StopAllCoroutines();

            _audioSource.PlayOneShot(_destroySound);
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

            foreach (MeshRenderer renderer in _meshRenderer)
                renderer.enabled = false;

            Destroy(gameObject, 2f);
            _isExploded = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletDestroy();
    }

    private void FLR(ShootFLR flr)
    {
        if(!_lfrResistent)
        {
            int index;
            index = Random.Range(0, 6);

            if(index == 0)
            {
                Target = flr.Target;
            }
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<ShootFLR>(FLR);
    }
}
