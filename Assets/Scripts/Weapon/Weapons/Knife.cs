using System.Collections;
using UnityEngine;
using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;

public class Knife : AbstractWeapon
{
    private EventBus _eventBus;

    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _mainCamera = Camera.main;  
    }

    private void OnEnable()
    {
        _mainCamera.enabled = true;
        _eventBus.Invoke(new SetTotalBullets(false));
        _eventBus.Invoke(new SetImage(3, true));

        _canShoot = false;
        Invoke("CanShoot", 0.8f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _eventBus.Invoke(new KnifeShootAnim());
            StartCoroutine(Shoot(_mainCamera));
        }
    }

    protected override IEnumerator Shoot(Camera cam)
    {
        yield return new WaitForSeconds(0.15f);

        Collider[] hit = Physics.OverlapSphere(cam.transform.position, _range);
        foreach(Collider enemies in hit)
        {
            if (enemies.TryGetComponent(out IEnemyHealth _health))
                _health.GetDamage(_damage);
        }
        yield break;
    }

    protected override void CanShoot()
    {
        base.CanShoot();
    }

    private void OnDisable()
    {
        _eventBus.Invoke(new SetImage(3, false));

        _canShoot = false;
        CancelInvoke("CanShoot");
    }
}
