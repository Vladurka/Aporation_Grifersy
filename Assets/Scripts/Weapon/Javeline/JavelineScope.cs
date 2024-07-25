using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class JavelineScope : MonoBehaviour
{
    private Camera _mainCamera;

    [HideInInspector] public Camera AimCamera;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _scopeCanvas;


    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _mainCamera = Camera.main;

        _mainCamera.enabled = true;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _mainCamera.enabled = false;
            _scopeCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = true;
            _eventBus.Invoke(new ChangeSens(70));
        }
        else
        {
            _mainCamera.enabled = true;
            _scopeCanvas.SetActive(false);
            _mainCanvas.SetActive(true);
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = false;
            _eventBus.Invoke(new ChangeSens(300));
        }
    }
}
