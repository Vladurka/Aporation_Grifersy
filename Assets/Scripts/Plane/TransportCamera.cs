using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class TransportCamera : MonoBehaviour
{
    [SerializeField ] private Transform _target;
    [SerializeField] private float _distance = 10.0f;
    [SerializeField] private float _xSpeed = 120.0f;
    [SerializeField] private float _ySpeed = 120.0f;

    [SerializeField] private float _yMinLimit = -20f;
    [SerializeField] private float _yMaxLimit = 80f;

    private Camera _camera;

    private float _x = 0.0f;
    private float _y = 0.0f;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _x = angles.y;
        _y = angles.x;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_camera.enabled)
        {
            _x += Input.GetAxis("Mouse X") * _xSpeed * 0.02f;
            _y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

            _y = ClampAngle(_y, _yMinLimit, _yMaxLimit);

            Quaternion rotation = Quaternion.Euler(_y, _x, 0);
            Vector3 desiredPosition = rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.position;

            RaycastHit hit;
            if (Physics.Linecast(_target.position, desiredPosition, out hit))
            {
                float adjustedDistance = Vector3.Distance(_target.position, hit.point) - 0.5f; 
                desiredPosition = rotation * new Vector3(0f, 0f, -adjustedDistance) + _target.position;
            }

            transform.rotation = rotation;
            transform.position = desiredPosition;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;

        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
    }
}
