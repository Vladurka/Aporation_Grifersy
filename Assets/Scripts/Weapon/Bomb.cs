using Game.Player;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    private AudioSource _source;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
       Explode();
    }

    private void Explode()
    {
        Instantiate(_effect, transform.position, transform.rotation);
        _source.Play();
        _meshRenderer.enabled = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hit in hits)
        {
            if (hit.transform.TryGetComponent(out IEnemyHealth enemy))
                enemy.GetDamage(100f);

            if (hit.transform.TryGetComponent(out ITargetHealth target) && !target.IsArmored)
                target.Destroy();
        }

        Destroy(gameObject, 2f);
    }
}
