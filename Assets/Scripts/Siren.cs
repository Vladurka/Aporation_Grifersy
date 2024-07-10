using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Siren : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetSiren>(SetSiren, 1);
    }

    private void SetSiren(SetSiren setSiren)
    {
        if(!_audioSource.isPlaying)
            _audioSource.Play();

        Invoke("SetDie", 5f);
    }

    private void SetDie()
    {
        _eventBus.Invoke(new SetDie());
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetSiren>(SetSiren);
    }
}
