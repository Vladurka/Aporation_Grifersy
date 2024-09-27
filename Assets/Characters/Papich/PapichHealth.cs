using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class PapichHealth : MonoBehaviour, IPapichHealth
{
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;
    private EventBus _eventBus;
    public float Health { get; set; } = 100f;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0f)
            Die();
    }

    public void Die()
    {
        _eventBus.Invoke(new SetDie());
    }

    public void Say()
    {
        int index = Random.Range(0, _clips.Length);

        if (!_audioSource.isPlaying)
            _audioSource.PlayOneShot(_clips[index]);
    }
}
