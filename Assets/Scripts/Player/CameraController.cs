using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class CameraController : MonoBehaviour, IService
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensitivaty = 300f;
    [SerializeField] private Transform _playerBody;

    private EventBus _eventBus;

    private float _rotationX = 0f;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ChangeSens>(GetSens, 1);

        Cursor.lockState = CursorLockMode.Locked;       
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivaty * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivaty * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -55f, 40f);

        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }

    private void GetSens(ChangeSens sens)
    {
        _sensitivaty = sens.Value;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<ChangeSens>(GetSens);
    }
}
