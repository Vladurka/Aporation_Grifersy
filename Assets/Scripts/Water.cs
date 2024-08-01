using Game.Player;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayerHealth player))
            player.GetDamage(100f);

        if (other.TryGetComponent(out IHelicopterHealth helicopter) && !_mainCharacter.activeSelf)
            helicopter.GetDamage(100f);

        if (other.TryGetComponent(out ICarHealth car) && !_mainCharacter.activeSelf)
            car.GetDamage(100f);

        if (other.gameObject.CompareTag("F-14"))
            _eventBus.Invoke(new SetDie());

    }
}
