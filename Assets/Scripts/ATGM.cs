using System.Collections;
using UnityEngine;

public class ATGM : MonoBehaviour
{
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _reloadingTime = 7f;

    [SerializeField] private Camera _cameraATGM;
    [SerializeField] private Camera _cameraMinigun;
    [SerializeField] private Canvas _canvas;

    private Camera _camera;

    private int _missiles = 2;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_cameraMinigun.enabled)
        {
            _cameraATGM.enabled = true;
            _camera.enabled = false;
            _canvas.enabled = true;
        }

        if (Input.GetMouseButtonUp(1) && !_cameraMinigun.enabled)
        {
            _camera.enabled = true;
            _cameraATGM.enabled = false;
            _canvas.enabled = false;
        }

        if (Input.GetMouseButtonDown(0) && _cameraATGM.enabled)
            LaunchMissile();
    }

    private void LaunchMissile()
    {
        if (_missiles > 0)
        {
            GameObject missile = Instantiate(_missilePrefab, _launchPoint.position, _launchPoint.rotation);
            MissileATGM missileScript = missile.GetComponent<MissileATGM>();
            missileScript.Camera = _cameraATGM;

            _missiles--;

            if (_missiles <= 0)
                StartCoroutine(Reload());

        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadingTime);
        _missiles = 2;
    }
}
