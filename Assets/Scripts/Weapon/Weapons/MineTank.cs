using UnityEngine;

public class MineTank : MonoBehaviour
{
    [SerializeField] private float _range = 15f;
    [SerializeField] private ParticleSystem _effect;
    private void Start()
    {
        SetMine.explode += Explode;
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider hit in hits)
        {
            if(hit.transform.TryGetComponent(out ITankHealth health))
                health.Destroy();
        }
        Instantiate(_effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SetMine.explode -= Explode;
    }

}
