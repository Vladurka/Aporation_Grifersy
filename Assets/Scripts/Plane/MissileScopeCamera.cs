using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MissileScopeCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float _maxXRotation = 45f;
    [SerializeField] private float _minXRotation = -45f;
    [SerializeField] private float _maxYRotation = -45f;
    [SerializeField] private float _minYRotation = -0;
    [SerializeField] private float _scopingForce = 40f;
    [SerializeField] private Camera _camera;

    private float xRotation = 0f;
    private float yRotation = 0f;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    private void Update()
    {
        if (_camera.enabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, _minXRotation, _maxXRotation);

            yRotation = Mathf.Clamp(yRotation, _minYRotation, _maxYRotation);

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

            if (scroll != 0f && _camera.fieldOfView <= 70f && _camera.fieldOfView >= 10f)
            {
                _camera.fieldOfView += -scroll * _scopingForce;

                if (_camera.fieldOfView <= 10f)
                    _camera.fieldOfView = 10.1f;

                if (_camera.fieldOfView >= 70f)
                    _camera.fieldOfView = 69.9f;
            }
        }
    }
}
