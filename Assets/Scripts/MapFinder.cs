using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class MapFinder : MonoBehaviour
{
    private Camera _camera;
    private bool _isCompleted;

    private EventBus _eventBus;

    private void Start()
    {
        _camera = Camera.main;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (!_isCompleted)
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 3f))
            {
                if(hit.collider.CompareTag("Map"))
                {
                    _eventBus.Invoke(new EndSignal());
                    _isCompleted = true;
                    Debug.Log("Map");
                }
            }
        }
    }
}
