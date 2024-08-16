using System.Collections;
using UnityEngine;

public class TankShoot : AbstractTank, IVehicleShoot
{
    [SerializeField] private GameObject _bullet;

    private AudioSource _audioSource;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }
    }

    protected override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(5f);

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(_spawnPoint.position, -_spawnPoint.forward, out hit, _range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Instantiate(_shootingEffect, _spawnPoint.position, Quaternion.identity);
                _audioSource.Play();
                targetPoint = hit.point;
                Vector3 dir = targetPoint - _spawnPoint.position;
                float x = Random.Range(-_spread, _spread);
                float y = Random.Range(-_spread, _spread);

                Vector3 dirWidthSpread = dir + new Vector3(x, y, 0);
                GameObject currentBullet = Instantiate(_bullet, _spawnPoint.position, _spawnPoint.rotation);
                currentBullet.transform.forward = dirWidthSpread.normalized;
                currentBullet.GetComponent<Rigidbody>().AddForce(dirWidthSpread.normalized * _shootForce, ForceMode.Impulse);
            }
        }
        StartCoroutine(Shoot());
    }

    public void Stop()
    {
        this.enabled = false;
    }
}
