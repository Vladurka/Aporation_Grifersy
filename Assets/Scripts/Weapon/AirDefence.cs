using System.Collections;
using UnityEngine;
public class AirDefence : MonoBehaviour
{
    [SerializeField] private float _range = 2000f;
    [SerializeField] private float _time = 15f;
    [SerializeField] private float _rotationSpeed = 5f;

    [SerializeField] private GameObject _missile;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnPosition;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    //private void Update()
    //{
    //    if (_target != null)
    //    {
    //        Vector3 direction = _target.transform.position - transform.position;
    //        direction.y = 0; 
    //        Quaternion rotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
    //    }
    //}

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_time);

        if (Vector3.Distance(transform.position, _target.position) <= _range)
        {
            GameObject missileObject = Instantiate(_missile, _spawnPosition.position, _spawnPosition.rotation);
            AirMissile missile = missileObject.GetComponent<AirMissile>();
            missile.Target = _target;
        }
        StartCoroutine(Shoot());
    }
}
