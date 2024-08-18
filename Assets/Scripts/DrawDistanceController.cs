using UnityEngine;
using UnityEngine.UI;

public class DrawDistanceController : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private GameObject _capitalObject;

    private Camera[] _cameras;
    private string _key = "DrawDistanceKey";

    private void Start()
    {
        _cameras = _capitalObject.GetComponentsInChildren<Camera>(true);

        if (_slider != null)
        {
            _slider.minValue = 1000f;
            _slider.maxValue = 10000f;

            if (PlayerPrefsSafe.HasKey(_key))
            {
                if (_slider != null)
                    _slider.value = PlayerPrefsSafe.GetFloat(_key);
            }
            if (!PlayerPrefsSafe.HasKey(_key))
                _slider.value = 3000f;
        }


        foreach (Camera camera in _cameras)
        {
            if (camera != null)
            {
                if (PlayerPrefsSafe.HasKey(_key))
                    camera.farClipPlane = PlayerPrefsSafe.GetFloat(_key);

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
    }

    private void OnDestroy()
    {
        if(_slider != null)
            PlayerPrefsSafe.SetFloat(_key, _slider.value);
    }
}
