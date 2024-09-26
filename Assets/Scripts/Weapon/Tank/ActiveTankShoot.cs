using UnityEngine;
using System.Collections;

public class ActiveTankShoot : AbstractTank, IVehicleShoot
{
    [SerializeField] private string _1targetTag;
    [SerializeField] private string _2targetTag;
    [SerializeField] private float _damage;

    private AudioSource _audioSource;

    private TankMove _tankMove;
    private Gun _gun;

    private RaycastHit _hit;

    private void Start()
    {
        _tankMove = GetComponentInParent<TankMove>();
        _audioSource = GetComponent<AudioSource>();
        _gun = GetComponentInChildren<Gun>();

        FindNewTarget();
        StartCoroutine(CheckTarget());
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (!_target.Equals(null))
        {
            Vector3 direction = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }
    }


    private IEnumerator CheckTarget()
    {
        if (_target.Equals(null))
            FindNewTarget();

        if (Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, out _hit, _range))
        {
            if (_hit.collider.TryGetComponent(out ITargetHealth target))  
                _tankMove.CanMove = false;

            else
                _tankMove.CanMove = true;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckTarget());
    }

    protected override IEnumerator Shoot()
    {
        if (_target.Equals(null))
            FindNewTarget();

        if (Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, out _hit, _range))
        {
            if (_hit.collider.TryGetComponent(out ITargetHealth target))
            {
                Instantiate(_shootingEffect, _spawnPoint.position, Quaternion.identity);
                _audioSource.Play();
                target.GetDamage(_damage);

                if (target.Health <= 0)
                    FindNewTarget();  
            }
        }

        float intreval = Random.Range(4f, 7f);
        yield return new WaitForSeconds(intreval);
        StartCoroutine(Shoot());
    }

    private void FindNewTarget()
    {
        _target = GameObject.FindGameObjectWithTag(_1targetTag);

        if (_target.Equals(null))
            _target = GameObject.FindGameObjectWithTag(_2targetTag);

        _tankMove.Target = _target;
        _gun.Target = _target;
    }

    public void Stop()
    {
        StopAllCoroutines();
        this.enabled = false;
    }
}
