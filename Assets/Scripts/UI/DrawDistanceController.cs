using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawDistanceController : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private GameObject _capitalObject;
    [SerializeField] private GameObject[] _camerasPrefs;

    private Camera[] _camerasItems;
    private Camera[] _cameras;

    private string _key = "DrawDistanceKey";

    private void Start()
    {
        _cameras = _capitalObject.GetComponentsInChildren<Camera>(true);

        List<Camera> camerasList = new List<Camera>();

        foreach (GameObject obj in _camerasPrefs)
        {
            Camera[] foundCameras = obj.GetComponentsInChildren<Camera>(true);
            camerasList.AddRange(foundCameras);
        }
        
        _camerasItems = camerasList.ToArray();

        _slider.minValue = 1000f;
        _slider.maxValue = 10000f;

        if (PlayerPrefs.HasKey(_key))
        {
            if (_slider != null)
                _slider.value = PlayerPrefsSafe.GetFloat(_key);
        }
        if (!PlayerPrefs.HasKey(_key))
            _slider.value = 3000f;


        foreach (Camera camera in _cameras)
        {
            if (camera != null)
            {
                if (PlayerPrefs.HasKey(_key))
                    camera.farClipPlane = PlayerPrefs.GetFloat(_key);

                if (!PlayerPrefs.HasKey(_key))
                    camera.farClipPlane = 3000f;
            }
        }

        foreach (Camera camera in _camerasItems)
        {
            if (camera != null)
            {
                if (PlayerPrefs.HasKey(_key))
                    camera.farClipPlane = PlayerPrefs.GetFloat(_key);

                if (!PlayerPrefsSafe.HasKey(_key))
                    camera.farClipPlane = 3000f;
            }
        }
    }

    private void Update()
    {
        foreach (Camera camera in _cameras)
        {
            if (camera != null && _slider != null)
                camera.farClipPlane = _slider.value;
        }

        foreach (Camera camera in _camerasItems)
        {
            if (camera != null && _slider != null)
                camera.farClipPlane = _slider.value;
        }
    }

    private void OnDestroy()
    {
        if(_slider != null)
            PlayerPrefs.SetFloat(_key, _slider.value);
    }
}
