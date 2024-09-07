using Game.Player;
using System.Collections;
using UnityEngine;

public class TankShoot : AbstractTank, IVehicleShoot
{
    private AudioSource _audioSource;

    private void Start()
    {
        if (!_target)
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
            Debug.DrawRay(_spawnPoint.position, _spawnPoint.forward * _range);
        }
    }

    protected override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(5f);

        RaycastHit hit;

        Vector3 spreadDirection = _spawnPoint.forward;
        spreadDirection.x += Random.Range(-_spread, _spread);
        spreadDirection.y += Random.Range(-_spread, _spread);

        if (Physics.Raycast(_spawnPoint.position, spreadDirection, out hit, _range))
        {
            if (hit.collider.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.GetDamage(100f);
                Instantiate(_shootingEffect, _spawnPoint.position, Quaternion.identity);
                _audioSource.Play();
            }
        }

        StartCoroutine(Shoot());
    }

    public void Stop()
    {
        StopAllCoroutines();
        this.enabled = false;
    }
}
