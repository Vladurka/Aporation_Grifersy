using System.Collections;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private Camera _thisCamera;
    private Camera _mainCamera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _range = 500f;
    [SerializeField] private AudioSource _source;

    private bool _isThisCamera = false;
    private bool _canShoot = true;

    RaycastHit hit;

    private void Start()
    {
        _mainCamera = Camera.main;
        _thisCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isThisCamera)
        {
            StartCoroutine(Shoot());

            if (!_source.isPlaying && _canShoot)
                _source.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();

            if (_source.isPlaying)
                _source.Stop();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_isThisCamera && !_scopeCamera.enabled)
            {
                _mainCamera.enabled = false;
                _isThisCamera = true;
                _scopeCamera.enabled = false;
                _thisCamera.enabled = true;
                _canvas.enabled = true;
            }

            else
            {
                _isThisCamera = false;
                _mainCamera.enabled = true;
                _thisCamera.enabled = false;
                _canvas.enabled = false;

                StopAllCoroutines();

                if (_source.isPlaying)
                    _source.Stop();
            }

        }

        if (!_scopeCamera.enabled)
            _canShoot = true;

        if (_scopeCamera.enabled)
            _canShoot = false;

    }

    private IEnumerator Shoot()
    {
        if (_canShoot)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, _range))
            {
                //if (hit.collider.TryGetComponent(out ITargetHealth target))
                //    target.GetDamage(3f);
            }

            yield return new WaitForSeconds(0.01f);
            StartCoroutine(Shoot());
        }

        else
            yield return null;
    }
}
