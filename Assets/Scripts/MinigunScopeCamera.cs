using UnityEngine;

public class MinigunScopeCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float _maxXRotation = 45f;
    [SerializeField] private float _minXRotation = -45f;
    [SerializeField] private float _maxYRotation = -45f;
    [SerializeField] private float _minYRotation = -0;
    [SerializeField] private float _scopingForce = 0.05f;
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject _minigun1;
    [SerializeField] private GameObject _minigun2;

    private float xRotation = 0f;
    private float yRotation = 0f;


    private void Update()
    {
        if (_camera.enabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, _minXRotation, _maxXRotation);

            yRotation = Mathf.Clamp(yRotation, _minYRotation, _maxYRotation);

            _minigun1.transform.localRotation = Quaternion.Euler(-180, yRotation, 0f);
            _minigun2.transform.localRotation = Quaternion.Euler(-xRotation, 0, 0f);

            if (Input.GetKey(KeyCode.E) && _camera.fieldOfView >= 10f)
                _camera.fieldOfView -= _scopingForce;

            if (Input.GetKey(KeyCode.Q) && _camera.fieldOfView <= 70f)
                _camera.fieldOfView += _scopingForce;
        }
    }
}
