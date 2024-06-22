using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class SetMine : AbstractWeapon
{
    [SerializeField] private GameObject _minePrefab;

    private EventBus _eventBus;
    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuyMine>(AddMines, 1);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(Shoot(_mainCamera));
        }
    }

    protected override IEnumerator Shoot(Camera cam)
    {
        if (TotalBullets > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, _range))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    Instantiate(_minePrefab, hit.point, transform.rotation);
                    TotalBullets--;
                }
            }
        }
            yield return null;
    }

    private void AddMines(BuyMine buyMine)
    {
        TotalBullets += buyMine.Amount;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyMine>(AddMines);
    }
}
