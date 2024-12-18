using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour, IService
{
    [SerializeField] private Grenade _grenade;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private float _throwForce;
    [SerializeField] private GameObject _grenadeModel;

    private bool _canShoot = true;

    private EventBus _eventBus;

    public int Grenades = 10;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnEnable()
    {
        _grenadeModel.SetActive(true);
        _eventBus.Subscribe<BuyGrenades>(AddGrenades, 1);
        _eventBus.Invoke(new UpdateTotalBullets(Grenades));
        _eventBus.Invoke(new SetTotalBullets(true));
        _eventBus.Invoke(new SetImage(2, true));

        _canShoot = false;
        Invoke("CanShoot", 0.8f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canShoot && Grenades > 0)
        {
            _eventBus.Invoke(new Throw1());
        }

        if (Input.GetMouseButtonUp(0) && _canShoot && Grenades > 0)
        {
            StartCoroutine(Shoot());
        }
    }

    protected IEnumerator Shoot()
    {
        _eventBus.Invoke(new Throw2());
        Instantiate(_grenade, _throwPoint.position, _throwPoint.rotation).Throw(_throwPoint.forward * _throwForce);
        _grenadeModel.SetActive(false);
        Grenades--;
        _canShoot = false;
        _eventBus.Invoke(new UpdateTotalBullets(Grenades));
        yield return null;
    }

    private void AddGrenades(BuyGrenades grenades)
    {
        Grenades += grenades.Amount;
        _eventBus.Invoke(new UpdateTotalBullets(Grenades));
    }

    private void CanShoot()
    {
        _canShoot = true;
    }

    private void OnDisable()
    {
        _eventBus.Invoke(new SetTotalBullets(false));
        _eventBus.Invoke(new SetImage(2, false));

        _canShoot = false;
        CancelInvoke("CanShoot");
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyGrenades>(AddGrenades);
        _eventBus.Invoke(new SetImage(3, false));
    }
}
