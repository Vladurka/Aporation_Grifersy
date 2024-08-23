using Game.Player;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;

    private EventBus _eventBus;
    private AudioSource _audioSource;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayerHealth player))
            player.Die();

        if (_mainCharacter != null)
        {
            if (other.TryGetComponent(out ITargetHealth transportHealth) && !_mainCharacter.activeSelf)
            {
                if (other.gameObject.CompareTag("Helicopter") || other.gameObject.CompareTag("Car"))
                    transportHealth.Destroy();
            }
        }

        if(other.TryGetComponent(out AbstractTransport transport) && other.gameObject.CompareTag("Drone"))
            transport.Exit();

        if (other.TryGetComponent(out ITargetHealth target))
        {
            if (other.gameObject.CompareTag("F-14"))
                _eventBus.Invoke(new SetDie());

            if (other.gameObject.CompareTag("Stryker"))
                target.Destroy();   
        }

        _audioSource.Play();
    }
}
