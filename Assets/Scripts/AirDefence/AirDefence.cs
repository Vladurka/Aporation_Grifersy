using System.Collections;
using UnityEngine;
public class AirDefence : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _missile;
    [SerializeField] private float _range = 2000f;
    [SerializeField] private string _tag = "F-14";
    [SerializeField] private float _interval = 15f;

    private GameObject _target;
    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(_tag);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_interval);

        if(Vector3.Distance(_target.transform.position, transform.position) <= _range)
        {
            _audioSource.PlayOneShot(_shootClip);
            Instantiate(_missile, _spawnPosition.position, _spawnPosition.rotation);
        }

        StartCoroutine(Shoot());
    }

}
