using UnityEngine;

public class MissileATGM : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _turnSpeed = 5f;
    [SerializeField] private ParticleSystem _explosionEffect;

    private Vector3 _targetPoint;

    public Camera Camera; 

    private void Update()
    {
        if (Camera.Equals(null))
        {
            Ray ray = Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                _targetPoint = hit.point;

            else
                _targetPoint = ray.GetPoint(1000);

            transform.position += transform.forward * _speed * Time.deltaTime;
            Vector3 direction = (_targetPoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _turnSpeed * Time.deltaTime);
        }
    }
}
