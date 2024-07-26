using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissileLouncher : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private List<GameObject> _missile;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject X3;
    [SerializeField] private int _missilesAmount = 4;

    [SerializeField] private Camera _scopeCamera;
    [SerializeField] private Camera _minigunCamera;

    [SerializeField] private Canvas _scopeCanvas;
    [SerializeField] private Canvas _minigunCanvas;
    [SerializeField] private GameObject _gameCanvas;

    [SerializeField] private AudioSource _audioSource;

    private Camera _mainCamera;
    private Transform _target;
    private bool _targetSet = false;
    private int _distance;

    public void Init()
    {
        _mainCamera = Camera.main;

        _mainCamera.enabled = true;

        X3.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && !_minigunCamera.enabled)
        {
            _mainCamera.enabled = false;
            _scopeCamera.enabled = true;
            _scopeCanvas.enabled = true;
            _gameCanvas.SetActive(false); 
            _minigunCanvas.enabled = false;
            _minigunCamera.enabled = false;
        }

        if (!Input.GetMouseButton(1))
        {
            _scopeCamera.enabled = false;
            _scopeCanvas.enabled = false;
            _gameCanvas.SetActive(true);
            _mainCamera.enabled = true;
        }

        if (Input.GetButtonDown("Fire1") && _scopeCamera.enabled == true)
        {
            Shoot();
        }

        if (_scopeCamera.enabled == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(_scopeCamera.transform.position, _scopeCamera.transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Plane") || hit.collider.CompareTag("AirDefence"))
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

    private void Shoot()
    {
        int index = 0;

        if (_missilesAmount > 0 && _target != null)
        {
            GameObject missileObject = Instantiate(_bullet, _spawnPosition.position, _spawnPosition.rotation);
            AirMissile missile = missileObject.GetComponent<AirMissile>();
            missile.Target = _target;

            _missile[index].SetActive(false);
            _missile.Remove(_missile[index]);
            
            _missilesAmount--;

            index++;
        }
    }
}