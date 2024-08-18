using Game.Player;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private ParticleSystem _effect;

    private GameObject _mainCharacter;
    private AudioSource _source;
    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _source = _mainCharacter.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayerHealth health))
        {
            health.GetDamage(100f);
            _source.PlayOneShot(_audioClip);
            Instantiate(_effect, transform.position, Quaternion.identity);
        }
    }
}
