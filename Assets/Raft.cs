using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class Raft : MonoBehaviour
{
    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefsSafe.SetInt("Prison", 1);
            _eventBus.Invoke(new PlayCut(2));
        }
    }
}
