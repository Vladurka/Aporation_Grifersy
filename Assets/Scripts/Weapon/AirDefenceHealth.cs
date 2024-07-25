using UnityEngine;
public class AirDefenceHealth : MonoBehaviour, ITargetHealth
{
    [SerializeField] private float _health = 100;
    [SerializeField] private ParticleSystem[] _lowHpEffect;
    [SerializeField] private Material _destoyedMaterial;

    private MeshRenderer[] _meshRenderer;

    private bool _gotDamage = false;

    private void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
        foreach (ParticleSystem effect in _lowHpEffect)
            effect.Stop();
    }

    public void GetDamage(float damage)
    {
        _health -= damage;

        if (!_gotDamage)
        {
            foreach (ParticleSystem effect in _lowHpEffect)
                effect.Play();
            _gotDamage = true;
        }

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        foreach (MeshRenderer mesh in _meshRenderer)
            mesh.material = _destoyedMaterial;

        if(transform.TryGetComponent(out AirDefence airDefence))
            airDefence.StopAllCoroutines();
    }
}

