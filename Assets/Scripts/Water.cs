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

        if (other.TryGetComponent(out IHelicopterHealth helicopter) && !_mainCharacter.activeSelf)
            helicopter.GetDamage(100f);

        if (other.TryGetComponent(out ICarHealth car) && !_mainCharacter.activeSelf)
            car.GetDamage(100f);

        if (other.gameObject.CompareTag("F-14"))
            _eventBus.Invoke(new SetDie());

        _audioSource.Play();

    }
}
