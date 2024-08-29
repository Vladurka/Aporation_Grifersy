using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

public class DroneLouncher : MonoBehaviour, IService
{
    [SerializeField] private Drone _drone;
    [SerializeField] private Transform _lounchPos;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject _panel;
    private GameObject _mainCharacter;

    public int DronesAmount = 1;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _mainCharacter = gameObject;
        _eventBus.Invoke(new UpdateDrone(DronesAmount));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && DronesAmount > 0)
        {
            if (!ConstSystem.InTransport && ConstSystem.CanPause && !ConstSystem.InShop)
            {
                Vector3 pos = transform.position;
                Drone newDrone = Instantiate(_drone, _lounchPos.position, _lounchPos.rotation);

                newDrone.MainCharacter = _mainCharacter;
                newDrone.GamePanel = _panel;
                newDrone.DroneSpawn = pos;
                newDrone.TextDistance = _distanceText;

                DronesAmount--;
                _eventBus.Invoke(new UpdateDrone(DronesAmount));
            }
        }
    }
}
