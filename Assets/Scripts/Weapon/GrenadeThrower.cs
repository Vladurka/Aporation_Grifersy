using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class GrenadeThrower : AbstractWeapon, IService
{
    [SerializeField] private Grenade _grenade;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private float _throwForce;
    [SerializeField] private GameObject _grenadeModel;

    private bool _canThrow;

    private EventBus _eventBus;

    public int Grenades = 10;
    public override void Init()
    {
        _mainCamera = Camera.main;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnEnable()
    {
        _grenadeModel.SetActive(true);
        _canThrow = true;
        _eventBus.Subscribe<BuyGrenades>(GetGrenades, 1);
        _eventBus.Invoke(new UpdateTotalBullets(Grenades));
        _eventBus.Invoke(new SetTotalBullets(true));
        _weaponImage.enabled = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canThrow == true && Grenades > 0)
        {
            _eventBus.Invoke(new Throw1());
        }

        if (Input.GetMouseButtonUp(0) && _canThrow == true && Grenades > 0)
        {
            StartCoroutine(Shoot(_mainCamera));
        }
    }

    protected override IEnumerator Shoot(Camera cam)
    {
        _eventBus.Invoke(new Throw2());
        Instantiate(_grenade, _throwPoint.position, _throwPoint.rotation).Throw(_throwPoint.forward * _throwForce);
        _grenadeModel.SetActive(false);
        Grenades--;
        _canThrow = false;
        _eventBus.Invoke(new UpdateTotalBullets(Grenades));
        yield return null;
    }

    private void GetGrenades(BuyGrenades grenades)
    {
        Grenades += grenades.Amount;
    }

    private void OnDisable()
    {
        _weaponImage.enabled = false;
        _eventBus.Invoke(new SetTotalBullets(false));
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyGrenades>(GetGrenades);
    }
}
