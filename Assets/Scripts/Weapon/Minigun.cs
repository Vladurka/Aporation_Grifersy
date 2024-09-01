using System.Collections;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField] private float _interval = 0.01f;
    [SerializeField] private float _shootForce = 10f;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _position;
    [SerializeField] private Transform _bulletPos;
    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private Camera _thisCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _range = 500f;
    [SerializeField] private AudioSource _source;

    private bool _isThisCamera = false;
    private bool _canShoot = true;

    RaycastHit hit;

    public void Init()
    {
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
            if (!_isThisCamera)
            {
                if (!_scopeCamera.enabled)
                {
                    _mainCamera.enabled = false;
                    _isThisCamera = true;
                    _scopeCamera.enabled = false;
                    _thisCamera.enabled = true;
                    _canvas.enabled = true;
                }
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
            if (Physics.Raycast(_position.position, _position.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out ITargetHealth target))
                    target.GetDamage(3f);
            }

            Ray ray = _thisCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75);

            Vector3 dirWithoutSpread = targetPoint - _bulletPos.position;

            GameObject currentBullet = Instantiate(_bullet, _bulletPos.position, _bulletPos.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirWithoutSpread.normalized * _shootForce, ForceMode.Impulse);
            Destroy(currentBullet, 5f);


            yield return new WaitForSeconds(_interval);
            StartCoroutine(Shoot());
        }

        else
            yield return null;
    }
}
