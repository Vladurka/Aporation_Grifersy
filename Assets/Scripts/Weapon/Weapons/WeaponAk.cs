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
        [SerializeField] private ParticleSystem _enemyEffect;

        [Header("Clips")]
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _noBulletsSound;
        [SerializeField] private AudioClip _reloadSound;

        [SerializeField] private int _maxBullets = 30;
        [SerializeField] private float _interval = 0.12f;

        private bool _canReaload = true;

        private EventBus _eventBus;

        public override void Init()
        {
            _mainCamera = Camera.main;

            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<BuyAkBullets>(AddBullets, 1);
            _eventBus.Subscribe<SetAimCamera>(GetCamera, 1);

            _aimCamera = GetComponentInChildren<Camera>();

            _audioSource = GetComponent<AudioSource>();

            _mainCamera.enabled = true;
            _aimCamera.enabled = false;
        }

        private void OnEnable()
        {
            _mainCamera.enabled = true;
            _shootingEffect.Stop();
            _eventBus.Invoke(new SetCurrentBullets(true));
            _eventBus.Invoke(new SetTotalBullets(true));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new SetImage(0, true));

            _canShoot = false;
            Invoke("CanShoot", 0.8f);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") )
            {
                if (_mainCamera.enabled)
                    StartCoroutine(Shoot(_mainCamera));

                if (_aimCamera.enabled)
                    StartCoroutine(Shoot(_aimCamera));
            }

            if (Input.GetMouseButtonUp(0))
                StopAllCoroutines();

            if (Input.GetKeyDown(KeyCode.R) && _canReaload && TotalBullets > 0 && Bullets < _maxBullets)
            {
                _eventBus.Invoke(new AkReloadAnim());
                _audioSource.PlayOneShot(_reloadSound);
                Invoke("Reaload", 1.5f);
                _canShoot = false;
                _canReaload = false;
            }
        }

        protected override IEnumerator Shoot(Camera _cam)
        {
            if (Bullets > 0 && _canShoot)
            {
                _eventBus.Invoke(new ShakeCamera(0.1f, 0.12f));
                _eventBus.Invoke(new AkShootAnim());

                RaycastHit hit;
                if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
                {

                    if (hit.collider.TryGetComponent(out IEnemyHealth enemy))
                    {
                        enemy.GetDamage(_damage);
                        Instantiate(_enemyEffect, hit.point, Quaternion.identity);
                    }

                    else if(!hit.collider.isTrigger)
                        Instantiate(_bulletEffect, hit.point, Quaternion.identity);
                }

                _audioSource.PlayOneShot(_shootSound);
                _shootingEffect.Play();
                Bullets--;

                _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
                _eventBus.Invoke(new CheckList(transform.position, _callRange));

                yield return new WaitForSeconds(_interval);

                StartCoroutine(Shoot(_cam));
            }

            if(Bullets <= 0)
            {
                _audioSource.PlayOneShot(_noBulletsSound);
                yield break;
            }
        }

        private void AddBullets(BuyAkBullets bullets)
        {
            TotalBullets += bullets.Amount;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
        }

        private void Reaload()
        {
            int bulletsToAdd = _maxBullets - Bullets;

            if (TotalBullets >= bulletsToAdd)
            {
                Bullets += bulletsToAdd; 
                TotalBullets -= bulletsToAdd; 
            }
            else
            {
                Bullets += TotalBullets; 
                TotalBullets = 0;
            }

            if (Bullets > _maxBullets)
                Bullets = _maxBullets;

            if (TotalBullets < 0)
                TotalBullets = 0;

            _canShoot = true;
            _canReaload = true;
            _eventBus.Invoke(new UpdateTotalBullets(TotalBullets));
            _eventBus.Invoke(new UpdateCurrentBullets(Bullets));
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
            _eventBus.Invoke(new SetCurrentBullets(false));
            _eventBus.Invoke(new SetTotalBullets(false));
            _eventBus.Invoke(new SetImage(3, false));

            _canShoot = false;
            CancelInvoke("CanShoot");
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<BuyAkBullets>(AddBullets);
            _eventBus.Unsubscribe<SetAimCamera>(GetCamera);
        }
    }
}
