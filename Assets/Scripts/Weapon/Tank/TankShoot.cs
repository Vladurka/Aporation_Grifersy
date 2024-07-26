using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject _mainCharacter;

    [SerializeField] private float _shootForce = 100;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _range = 250f;

    private float _spread = 1f;
    private bool _isStared = false;

    private AudioSource _audioSource;

    private EventBus _eventBus;
    private void Start()
    {
        if (_mainCharacter == null)
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");

        _audioSource = GetComponent<AudioSource>();
        _eventBus  = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<DestroyTank>(DestroyTank, 1);
        StartCoroutine(Shoot());

        _isStared = true;
    }

    private void Update()
    {
        if (_mainCharacter != null)
        {
            Vector3 direction = _mainCharacter.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        };
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(5f);

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(_spawnBullet.position, -_spawnBullet.forward, out hit, _range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Instantiate(_effect, _spawnBullet.position, Quaternion.identity);
                _audioSource.Play();
                targetPoint = hit.point;
                Vector3 dir = targetPoint - _spawnBullet.position;
                float x = Random.Range(-_spread, _spread);
                float y = Random.Range(-_spread, _spread);

                Vector3 dirWidthSpread = dir + new Vector3(x, y, 0);
                GameObject currentBullet = Instantiate(_bullet, _spawnBullet.position, _spawnBullet.rotation);
                currentBullet.transform.forward = dirWidthSpread.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(dirWidthSpread.normalized * _shootForce, ForceMode.Impulse);
            }
        }
        StartCoroutine(Shoot());
    }

    private void DestroyTank(DestroyTank tank)
    {
        StopAllCoroutines();
        this.enabled = false;
    }

    private void OnDestroy()
    {
        if(_isStared)
            _eventBus.Unsubscribe<DestroyTank>(DestroyTank);
    }
}
