using UnityEngine;
using UnityEngine.UI;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class Scoping : MonoBehaviour
{
    [SerializeField] private Image _crosshair;

    private Camera _mainCamera;

    [HideInInspector] public Camera AimCamera;

    private EventBus _eventBus;
    private Movement _movement;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _movement = ServiceLocator.Current.Get<Movement>();

        _mainCamera = Camera.main;
        _crosshair.enabled = true;

        _mainCamera.enabled = true;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _mainCamera.enabled = false;
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = true;
            _eventBus.Invoke(new SetAimCamera(AimCamera));
            _eventBus.Invoke(new ChangeSens(70));
            _movement.Speed = _movement.ScopeSpeed;
            _movement.InScope = true;
            _crosshair.enabled = false;
        }
        else
        {
            _mainCamera.enabled = true;
            AimCamera = GetComponentInChildren<Camera>();
            AimCamera.enabled = false;
            _eventBus.Invoke(new ChangeSens(300));
            _movement.Speed = _movement.NormalSpeed;
            _movement.InScope = false;
            _crosshair.enabled = true;
        }
    }
}
