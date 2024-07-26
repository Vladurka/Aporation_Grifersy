using UnityEngine;
public class PlaneScopeCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float _maxXRotation = 45f;
    [SerializeField] private float _minXRotation = -45f;
    [SerializeField] private float _maxYRotation = 45f;
    [SerializeField] private float _minYRotation = -45f;
    [SerializeField] private Camera _camera;

    private float xRotation = 0f;
    private float yRotation = 0f;


    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, _minXRotation, _maxXRotation);

        yRotation = Mathf.Clamp(yRotation, _minYRotation, _maxYRotation);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (_camera.enabled)
        {
            if (Input.GetKey(KeyCode.E) && _camera.fieldOfView >= 10f)
                _camera.fieldOfView -= 0.05f;

            if (Input.GetKey(KeyCode.Q) && _camera.fieldOfView <= 70f)
                _camera.fieldOfView += 0.05f;
        }


    }
}
