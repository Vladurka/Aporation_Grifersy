using System;
using System.Collections;
using UnityEngine;
using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;

namespace Game.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class RPG : AbstractWeapon, IService
    {
        [Header("Require to fill")]
        [SerializeField] private GameObject _staticBullet;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _spawnBullet;

        [SerializeField] private float _shootForce = 1f;

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
            Realod();
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
            if (Bullets > 0)
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

                float x = 0;
                float y = 0;

                Vector3 dirWidthSpread = dirWithoutSpread + new Vector3(x, y, 0);
                GameObject currentBullet = Instantiate(_bullet, _spawnBullet.position, _spawnBullet.rotation);

                currentBullet.transform.forward = dirWidthSpread.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(dirWidthSpread.normalized * _shootForce, ForceMode.Impulse);
                _staticBullet.SetActive(false);
                Bullets--;

                _eventBus.Invoke(new CheckList(transform.position, _callRange));

                yield return null;
            }
        }

        private void Realod()
        {
            Bullets = 1;
            _staticBullet.SetActive(true);
        }

        private void AddBullets(BuyRpgBullets bullets)
        {
            TotalBullets += bullets.Amount;
        }

        private void GetCamera(SetAimCamera AimCamera)
        {
            _aimCamera = AimCamera.AimCamera;
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyRpgBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
