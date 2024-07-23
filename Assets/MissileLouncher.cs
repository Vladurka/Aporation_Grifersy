using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissileLouncher : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPosition;
    [SerializeField] private List<GameObject> _missile;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject X3;
    [SerializeField] private int _missilesAmount = 4;
    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AudioSource _audioSource;

    private Camera _mainCamera;
    private Transform _target;
    private bool _targetSet = false;
    private int _distance;

    private void Start()
    {
        _mainCamera = Camera.main;

        _mainCamera.enabled = true;

        X3.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _scopeCamera.enabled = true;
            _canvas.enabled = true;
        }

        if (!Input.GetMouseButton(1))
        {
            _scopeCamera.enabled = false;
            _canvas.enabled = false;
        }

        if (Input.GetButtonDown("Fire1") && _scopeCamera.enabled == true)
        {
            StartCoroutine(Shoot());
        }

        if (_scopeCamera.enabled == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(_scopeCamera.transform.position, _scopeCamera.transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Plane"))
                {
                    if (!_targetSet)
                    {
                        _target = hit.collider.transform;
                        X3.SetActive(true);
                        _targetSet = true;
                    }

                    if (!_audioSource.isPlaying)
                        _audioSource.Play();
                }

                _distance = (int)hit.distance;

                _distanceText.text = _distance.ToString() + "m";
            }

            else
            {
                if (_targetSet)
                {
                    _target = null;
                    X3.SetActive(false);
                    _targetSet = false;
                }

                if (_audioSource.isPlaying)
                    _audioSource.Stop();

                _distanceText.text = "-";
            }
        }

        else
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
    }

    private IEnumerator Shoot()
    {
        if (_missilesAmount > 0 && _target != null)
        {
            int index = Random.Range(0, _missile.Count + 1);

            GameObject missileObject = Instantiate(_bullet, _spawnPosition[index].position, _spawnPosition[index].rotation);
            Missile missile = missileObject.GetComponent<Missile>();
            missile.Target = _target;

            _missile[index].SetActive(false);
            _missile.Remove(_missile[index]);
            _spawnPosition.Remove(_spawnPosition[index]);
            
            _missilesAmount--;
        }
       
        yield return null;
    }
}