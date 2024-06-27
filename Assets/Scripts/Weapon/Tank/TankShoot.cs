using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _spawnBullet;

    [SerializeField] private float _shootForce = 100;
    [SerializeField] private float _rotationSpeed = 2f;

    private GameObject _mainCharacter;

    private EventBus _eventBus;
    private void Start()
    {
        _eventBus  = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<DestroyTank>(DestroyTank, 1);
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shoot());
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
        }
    }

    private IEnumerator Shoot()
    {
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(_gun.transform.position, transform.forward, out hit))
        {
            Instantiate(_effect, _spawnBullet.position, Quaternion.identity);
            targetPoint = hit.point;
            Vector3 dirWithoutSpread = targetPoint - _spawnBullet.position;
            GameObject currentBullet = Instantiate(_bullet, _spawnBullet.position, _spawnBullet.rotation);
            currentBullet.transform.forward = dirWithoutSpread.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(dirWithoutSpread.normalized * _shootForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(5f);
        StartCoroutine(Shoot());
    }

    private void DestroyTank(DestroyTank tank)
    {
        StopAllCoroutines();
        this.enabled = false;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<DestroyTank>(DestroyTank);
    }
}
