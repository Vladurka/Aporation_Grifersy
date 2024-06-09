using UnityEngine;

public class AirMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _explodeRadius = 5f;

    private GameObject _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Helicopter");
    }

    void Update()
    {
        Vector3 direction = _target.transform.position - transform.position;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, _target.transform.position) <= _explodeRadius)
            Destroy(gameObject);

    }

}
