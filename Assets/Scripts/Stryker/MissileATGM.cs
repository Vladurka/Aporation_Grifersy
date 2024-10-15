using UnityEngine;

public class MissileATGM : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _turnSpeed = 5f;
    [SerializeField] private ParticleSystem _explosionEffect;

    private Vector3 _targetPoint;

    public Camera Camera;

    void Update()
    {
        if (Camera != null)
        {
            Ray ray = Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                _targetPoint = hit.point;

            else
                _targetPoint = transform.position + transform.forward * 1000f;

            transform.position += transform.forward * _speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, _targetPoint) > 0.1f)
            {
                Vector3 direction = (_targetPoint - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float maxRotationAngle = 30f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle * Time.deltaTime * _turnSpeed);
            }
        }
    }
}
