using UnityEngine;
using Game.Weapon;
using Game.SeniorEventBus;
using Game.Player;
using Game.Data;
namespace Game
{
    public class EnterLevel : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private GameObject _mainCharacter;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private Movement _movement;
        [SerializeField] private CameraController _cameraController;

        [Header("Helicopter")]
        [SerializeField] private Helicopter _helicopter;
        [SerializeField] private HelicopterStatesController _helicopterStatesController;

        [Header("Car")]
        [SerializeField] private Car _car;
        [SerializeField] private ChangeSkinCar _changeSkinCar;

        [Header("Weapon")]
        [SerializeField] private WeaponAk _weaponAk;
        [SerializeField] private RPG _rpg;
        [SerializeField] private Knife _knife;
        [SerializeField] private ScopeLevels _scopeLevels;
        [SerializeField] private ChangeRpgSkin _changeRpgSkin;
        [SerializeField] private ChangeAkSkin _changeAkSkin;
        [SerializeField] private ChangeKnifeSkin _changeKnifeSkin;
        [SerializeField] private GrenadeThrower _grenadeThrower;
        [SerializeField] private ChangeWeapon _changeWeapon;

        [Header("Animations")]
        [SerializeField] private AKAnim _akAnim;
        [SerializeField] private RPGAnim _rpgAnim;
        [SerializeField] private GrenadeAnim _grenadeAnim;
        [SerializeField] private KnifeAnim _knifeAnim;

        [Header("UI")]
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private GamePanel _gamePanel;
        [SerializeField] private VolumeController _volume;

        [Header("Else")]
        [SerializeField] private Raycasts _raycasts;
        [SerializeField] private BaseStates _baseStates;
        [SerializeField] private JSON_load _loadDataJson;
        [SerializeField] private JSON_save _saveDataJson;
        [SerializeField] private Shop _shop;
        [SerializeField] private MissionsController _missionsController;
        [SerializeField] private Loading _loading;
        [SerializeField] private PapichMovement _papich;
        [SerializeField] private DroneLouncher _droneLouncher;

        [SerializeField] private bool _load = true;

        private EnemyListController _enemyListController;
        private EventBus _eventBus;
        private CoinSystem _coinSystem;

        private void Awake()
        {
            _eventBus = new EventBus();
            _enemyListController = new EnemyListController();
            _coinSystem = new CoinSystem();

           Register();
           Init();

           RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
           float targetFPS = refreshRate.numerator;

           if (PlayerPrefsSafe.HasKey("FPS"))
               Application.targetFrameRate = PlayerPrefsSafe.GetInt("FPS");

           if (!PlayerPrefsSafe.HasKey("FPS"))
               Application.targetFrameRate = (int)targetFPS;
        }

        private void Init()
        {
            _mainCharacter.SetActive(true);

            _loadDataJson.Init();

            if(_load)
                _loadDataJson.Load();

            _saveDataJson.Init();

            _papich.Init();

            _gamePanel.Init();
            _playerHealth.Init();
            _helicopterStatesController.Init();
            _movement.Init();
            _helicopter.Init();
            _car.Init();
            _weaponAk.Init();
            _rpg.Init();
            _knife.Init();
            _enemyListController.Init();
            _raycasts.Init();
            _baseStates.Init();
            _cameraController.Init();
            _coinSystem.Init();
            _scopeLevels.Init();
            _shop.Init();
            _changeAkSkin.Init();
            _changeKnifeSkin.Init();
            _grenadeThrower.Init();
            _changeSkinCar.Init();
            _changeRpgSkin.Init();
            _missionsController.Init();
            _droneLouncher.Init();

            _akAnim.Init();
            _rpgAnim.Init();
            _grenadeAnim.Init();
            _knifeAnim.Init();

            _gameUI.Init();

            _changeWeapon.Init();

            Debug.Log("Initializated");
        }

        private void Register()
        {
            ServiceLocator.Initialize();

            ServiceLocator.Current.Register(_eventBus);
            ServiceLocator.Current.Register<PlayerHealth>(_playerHealth);
            ServiceLocator.Current.Register<Movement>(_movement);
            ServiceLocator.Current.Register<Helicopter>(_helicopter);
            ServiceLocator.Current.Register<Car>(_car);
            ServiceLocator.Current.Register<ChangeWeapon>(_changeWeapon);
            ServiceLocator.Current.Register<RPG>(_rpg);
            ServiceLocator.Current.Register<WeaponAk>(_weaponAk);
            ServiceLocator.Current.Register<Raycasts>(_raycasts);
            ServiceLocator.Current.Register<CoinSystem>(_coinSystem);
            ServiceLocator.Current.Register<BaseStates>(_baseStates);
            ServiceLocator.Current.Register<HelicopterStatesController>(_helicopterStatesController);
            ServiceLocator.Current.Register<CameraController>(_cameraController);
            ServiceLocator.Current.Register<ScopeLevels>(_scopeLevels);
            ServiceLocator.Current.Register<GrenadeThrower>(_grenadeThrower);
            ServiceLocator.Current.Register<VolumeController>(_volume);
            ServiceLocator.Current.Register<Shop>(_shop);
            ServiceLocator.Current.Register<Loading>(_loading);
            ServiceLocator.Current.Register<DroneLouncher>(_droneLouncher);
            Debug.Log("Registreted");
        }
    }
}
