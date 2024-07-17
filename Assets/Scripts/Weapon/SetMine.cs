using System;
using System.Collections;
using UnityEngine;

public class SetMine : AbstractWeapon
{
    [SerializeField] private GameObject _minePrefab;

    public static Action explode;

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

        if (Input.GetKeyDown(KeyCode.X))
        {
            explode?.Invoke();
        }
    }

    protected override IEnumerator Shoot(Camera cam)
    {
        if (TotalBullets > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, _range))
            {
                if (hit.collider.CompareTag("Tank"))
                {
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    Instantiate(_minePrefab, hit.point, rotation);
                    TotalBullets--;
                }
            }
        }

        yield return null;
    }
}
