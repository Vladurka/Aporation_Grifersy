using UnityEngine;
using UnityEngine.UI;
using Game.SeniorEventBus;

public class AbstractDroneLouncher : MonoBehaviour
{
    [SerializeField] protected BaseDrone _drone;
    [SerializeField] protected KamikadzeDrone _kamikadze;
    [SerializeField] protected FPVDrone _fpv;
    [SerializeField] protected Transform _lounchPos;
    [SerializeField] protected Text _distanceText;
    [SerializeField] protected Text _batteryText;
    [SerializeField] protected Text _nameText;
    [SerializeField] protected GameObject _panel;
    [SerializeField] protected float _maxDistance = 400f;
    protected GameObject _mainCharacter;

    [SerializeField] protected string _droneName1 = "Drone1";
    [SerializeField] protected string _droneName2 = "Drone2";
    public int DronesAmount = 1;

    protected EventBus _eventBus;
}
