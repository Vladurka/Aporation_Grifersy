using System.Collections;
using UnityEngine;

public class ActiveTankShoot : AbstractTank, IVehicleShoot
{
    [SerializeField] private string _1targetTag;
    [SerializeField] private string _2targetTag;

    [SerializeField] private float _damage;

    private TankMove _tankMove;

    private void Start()
    {
        _tankMove = GetComponentInParent<TankMove>();   

        _target = GameObject.FindGameObjectWithTag(_1targetTag);
        _tankMove.Target = _target;
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

    protected override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(5f);

        RaycastHit hit;

        if (Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, out hit, _range))
        {
            if (hit.collider.TryGetComponent(out ITargetHealth target))
            {
                _tankMove.CanMove = false;

                target.GetDamage(_damage);

                if (target.Health <= 0)
                {
                    _target = GameObject.FindGameObjectWithTag(_1targetTag);

                    if(_target == null)
                        _target = GameObject.FindGameObjectWithTag(_2targetTag);

                    _tankMove.Target = _target;
                }

                else
                    _tankMove.CanMove = true;
            }

            else
                _tankMove.CanMove = true;
        }

        StartCoroutine(Shoot());
    }

    public void Stop()
    {
        StopAllCoroutines();
        this.enabled = false;
    }
}
