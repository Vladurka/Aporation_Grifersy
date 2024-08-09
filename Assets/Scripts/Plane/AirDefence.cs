using System.Collections;
using UnityEngine;
public class AirDefence : MonoBehaviour
{
    [SerializeField] private float _range = 2000f;

    [SerializeField] private GameObject _missile;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _spawnPosition;

    private float _time;

    private void Start()
    {
        _time = Random.Range(15, 26);
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_time);

        if (Vector3.Distance(transform.position, _target.position) <= _range)
        {
            GameObject missileObject = Instantiate(_missile, _spawnPosition.position, _spawnPosition.rotation);
            Missile missile = missileObject.GetComponent<Missile>();
            missile.Target = _target;
        }

        _time = Random.Range(15, 26);
        StartCoroutine(Shoot());
    }
}
