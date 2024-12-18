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
            _mainCamera.enabled = true;

            if (TotalBullets > 0)
                Realod();

            _eventBus.Invoke(new SetTotalBullets(true));
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new SetImage(1, true));

            _canShoot = false;
            Invoke("CanShoot", 0.8f);
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1") && _mainCamera.enabled)
                StartCoroutine(Shoot(_mainCamera));

            if (Input.GetMouseButtonDown(0) && _aimCamera.enabled)
                StartCoroutine(Shoot(_aimCamera));
        }

        protected override IEnumerator Shoot(Camera cam)
        {
            if (TotalBullets > 0 && _canShoot)
            {
                _audioSource.Play();
                _eventBus.Invoke(new ShakeCamera(0.5f, 0.5f));
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
                _audioSource.PlayOneShot(_noBulletsSound);

            yield break;
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

        protected override void CanShoot()
        {
            base.CanShoot();
        }

        private void OnDisable()
        {
            _eventBus.Invoke(new SetTotalBullets(false));
            _eventBus.Invoke(new SetImage(1, false));

            _canShoot = false;
            CancelInvoke("CanShoot");
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyRpgBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
