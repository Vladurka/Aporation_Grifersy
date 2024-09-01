using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Javeline : AbstractWeapon
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject X3;

    private Transform _target;
    private bool _targetSet = false;
    private int _distance;

    private AudioSource _audioSource;
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _mainCamera = Camera.main;

        _audioSource = GetComponent<AudioSource>();

        _mainCamera.enabled = true;

        X3.SetActive(false);

        _eventBus.Invoke(new SetTotalBullets(true));
        _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
    }

    public override void Init()
    {

    }
    private void OnEnable()
    {
        _mainCamera.enabled = true;

        if (TotalBullets > 0)
            Realod();

        _eventBus.Invoke(new SetTotalBullets(true));
        _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _mainCamera.enabled == true)
        {
            StartCoroutine(Shoot(_mainCamera));
        }

        if (Input.GetButtonDown("Fire1") && _scopeCamera.enabled == true)
        {
            StartCoroutine(Shoot(_scopeCamera));
        }

        if (_scopeCamera.enabled == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(_scopeCamera.transform.position, _scopeCamera.transform.forward, out hit, _range))
            {
                if (hit.collider.CompareTag("Plane") && _canShoot)
                {
                    if (!_targetSet)
                    {
                        _target = hit.collider.transform;
                        X3.SetActive(true);
                        _targetSet = true;
                    }

                    if (!_audioSource.isPlaying)
                        _audioSource.Play();
                }

                _distance = (int)hit.distance;

                _distanceText.text = _distance.ToString() + "m";
            }

            else
            {
                if (_targetSet)
                {
                    _target = null;
                    X3.SetActive(false);
                    _targetSet = false;
                }

                if (_audioSource.isPlaying)
                    _audioSource.Stop();

                _distanceText.text = "-";
            }
        }

        else
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
    }

    protected override IEnumerator Shoot(Camera cam)
    {
        if(_target != null)
        {
            if (TotalBullets > 0 && _canShoot == true)
            {
                GameObject missileObject = Instantiate(_bullet, _spawnPosition.position, _spawnPosition.rotation);
                Missile missile = missileObject.GetComponent<Missile>();
                missile.Target = _target;
                TotalBullets--;
                _canShoot = false;
                _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            }
        }

        else if (_target == null)
        {
            if (TotalBullets > 0 && _canShoot == true)
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Vector3 targetPoint;

                if (Physics.Raycast(ray, out hit))
                    targetPoint = hit.point;
                else
                    targetPoint = ray.GetPoint(75);

                Vector3 dirWithoutSpread = targetPoint - _spawnPosition.position;

                GameObject currentBullet = Instantiate(_bullet, _spawnPosition.position, _spawnPosition.rotation);
                currentBullet.transform.forward = dirWithoutSpread.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(dirWithoutSpread.normalized * 1f, ForceMode.Impulse);
                TotalBullets--;
                _canShoot = false;
                _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            }
        }

        yield  return null;
    }

    private void Realod()
    {
        _canShoot = true;
    }

    private void OnDisable()
    {
        _eventBus.Invoke(new SetTotalBullets(false));
    }
}
