using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class BaseDroneLouncher : AbstractDroneLouncher, IService
{
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<BuyDrone>(BuyDrone, 1);

        _mainCharacter = gameObject;
        _eventBus.Invoke(new UpdateDrone(DronesAmount));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && DronesAmount > 0)
        {
            if (!ConstSystem.InTransport && ConstSystem.CanPause && !ConstSystem.IsBeasy)
            {
                Vector3 pos = transform.position;
                BaseDrone newDrone = Instantiate(_drone, _lounchPos.position, _lounchPos.rotation);

                newDrone.MainCharacter = _mainCharacter;
                newDrone.GamePanel = _panel;
                newDrone.DroneSpawn = pos;
                newDrone.TextDistance = _distanceText;
                newDrone.BatteryText = _batteryText;
                newDrone.CharacterPos = new Vector3(transform.position.x, transform.position.y - 0.77f, transform.position.z);
                newDrone.MaxDistance = _maxDistance;

                _nameText.text = _droneName1.ToString();
                DronesAmount--;
                _eventBus.Invoke(new UpdateDrone(DronesAmount));
            }
        }
    }

    private void BuyDrone(BuyDrone drone)
    {
        DronesAmount++;
        _eventBus.Invoke(new UpdateDrone(DronesAmount));
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<BuyDrone>(BuyDrone);
    }
}
