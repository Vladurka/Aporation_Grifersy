using Game.Player;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayerHealth health))
        {
            health.GetDamage(100f);
            _audioSource.Play();
            Instantiate(_effect, transform.position, Quaternion.identity);
        }
    }
}
