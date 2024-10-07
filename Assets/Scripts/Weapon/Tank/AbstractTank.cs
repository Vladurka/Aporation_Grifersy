using System.Collections;
using UnityEngine;

public abstract class AbstractTank : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _shootingEffect;
    [SerializeField] protected Transform _spawnPoint;

    [SerializeField] protected float _rotationSpeed = 2f;
    [SerializeField] protected float _range = 250f;
    [SerializeField] protected float _spread = 1f;

    [SerializeField] protected GameObject _target;

    protected abstract IEnumerator Shoot();
}
