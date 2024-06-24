using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

public class SetMine : AbstractWeapon
{
    [SerializeField] private GameObject _minePrefab;

    public override void Init()
    {
    }

    private void Start()
    {
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
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Instantiate(_minePrefab, hit.point, rotation);
                TotalBullets--;
            }
        }

        yield return null;
    }
}
