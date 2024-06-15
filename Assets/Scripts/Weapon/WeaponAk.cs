using UnityEngine;
using System.Collections;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

namespace Game.Weapon
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponAk : AbstractWeapon, IService
    {
        private AudioSource _audioSource;

        [Header("Effect")]
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private ParticleSystem _bulletEffect;

        [Header("Clips")]
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _noBulletsSound;

        [SerializeField] private int _maxBullets = 30;

        private bool _canShoot = true;

        private EventBus _eventBus;

        public override void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<BuyAkBullets>(AddBullets, 1);
            _eventBus.Subscribe<SetAimCamera>(GetCamera, 1);

            _mainCamera = Camera.main;
            _aimCamera = GetComponentInChildren<Camera>();

            _audioSource = GetComponent<AudioSource>();

            _mainCamera.enabled = true;
            _aimCamera.enabled = false;
        }

        private void OnEnable()
        {
            _mainCamera.enabled = true;
            _eventBus.Invoke(new SetCurrentBullets(true));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") )
            {
                if (_mainCamera.enabled == true)
                {
                    StartCoroutine(Shoot(_mainCamera));
                }

                else
                {
                    StartCoroutine(Shoot(_aimCamera));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.R) && Bullets < _maxBullets)
            {
                _eventBus.Invoke(new AkReloadAnim());
                _canShoot = false;
                Invoke("Reaload", 1.5f);
            }
        }

        protected override IEnumerator Shoot(Camera _cam)
        {
            if (Bullets > 0 && _canShoot == true)
            {
                _eventBus.Invoke(new AkShootAnim());

                RaycastHit hit;
                if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit))
                {
                    if (hit.collider.TryGetComponent(out IEnemyHealth enemy))
                    {
                        enemy.GetDamage(_damage);
                    }

                    else
                        Instantiate(_bulletEffect, hit.point, Quaternion.identity);
                }

                _audioSource.PlayOneShot(_shootSound);
                Instantiate(_shootingEffect, _spawnPosition.position, _spawnPosition.transform.rotation);
                Bullets--;

                _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
                _eventBus.Invoke(new CheckList(transform.position, _callRange));

                yield return new WaitForSeconds(0.15f);

                StartCoroutine(Shoot(_cam));
            }

            else
            {
                _audioSource.PlayOneShot(_noBulletsSound);
                yield return null;
            }
        }

        private void AddBullets(BuyAkBullets bullets)
        {
            TotalBullets += bullets.Amount;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
        }

        private void Reaload()
        {
            if (TotalBullets >= _maxBullets)
            {
                TotalBullets -= _maxBullets;
                Bullets = _maxBullets;
            }

            if (TotalBullets < _maxBullets)
            {
                Bullets = TotalBullets;
                TotalBullets = 0;
            }

            _canShoot = true;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
        }

        private void GetCamera(SetAimCamera AimCamera)
        {
            _aimCamera = AimCamera.AimCamera;
        }

        private void OnDisable()
        {
            _eventBus.Invoke(new SetCurrentBullets(false));
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyAkBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
