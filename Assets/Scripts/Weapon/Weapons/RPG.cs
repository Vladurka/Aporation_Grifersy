using System.Collections;
using UnityEngine;
using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;

namespace Game.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class RPG : AbstractWeapon, IService
    {
        [SerializeField] private GameObject _staticBullet;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _spawnBullet;

        [SerializeField] private float _shootForce = 1f;

        [SerializeField] private AudioClip _noBulletsSound;

        private bool _canShoot = true;

        private AudioSource _audioSource;

        private EventBus _eventBus;

        public override void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<BuyRpgBullets>(AddBullets, 1);
            _eventBus.Subscribe<SetAimCamera>(GetCamera, 2);

            _mainCamera = Camera.main;

            _audioSource = GetComponent<AudioSource>();

            _mainCamera.enabled = true;
        }

        private void OnEnable()
        {
            if (TotalBullets > 0)
            {
                _mainCamera.enabled = true;
                Realod();
            }

            _eventBus.Invoke(new SetTotalBullets(true));
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new SetImage(1, true));
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1") && _mainCamera.enabled == true)
            {
                StartCoroutine(Shoot(_mainCamera));
            }

            if (Input.GetMouseButtonDown(0) && _aimCamera.enabled == true)
            {
                StartCoroutine(Shoot(_aimCamera));
            }
        }

        protected override IEnumerator Shoot(Camera cam)
        {
            if (TotalBullets > 0 && _canShoot == true)
            {
                _audioSource.Play();
                _eventBus.Invoke(new RpgShootAnim());

                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                Vector3 targetPoint;

                if (Physics.Raycast(ray, out hit))
                    targetPoint = hit.point;
                else
                    targetPoint = ray.GetPoint(75);

                Vector3 dirWithoutSpread = targetPoint - _spawnBullet.position;

                GameObject currentBullet = Instantiate(_bullet, _spawnBullet.position, _spawnBullet.rotation);
                currentBullet.transform.forward = dirWithoutSpread.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(dirWithoutSpread.normalized * _shootForce, ForceMode.Impulse);

                //_eventBus.Invoke(new InstantRocket(dirWithoutSpread, _shootForce));

                _staticBullet.SetActive(false);
                _canShoot = false;
                TotalBullets--;

                _eventBus.Invoke(new CheckList(transform.position, _callRange));
                _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
                
            }

            if (TotalBullets <= 0)
            {
                _audioSource.PlayOneShot(_noBulletsSound);
            }

            yield return null;
        }

        private void Realod()
        {
            _staticBullet.SetActive(true);
            _canShoot = true;
        }

        private void AddBullets(BuyRpgBullets bullets)
        {
            TotalBullets += bullets.Amount;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
        }

        private void GetCamera(SetAimCamera AimCamera)
        {
            _aimCamera = AimCamera.AimCamera;
        }

        private void OnDisable()
        {
            _eventBus.Invoke(new SetTotalBullets(false));
            _eventBus.Invoke(new SetImage(1, false));
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyRpgBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
