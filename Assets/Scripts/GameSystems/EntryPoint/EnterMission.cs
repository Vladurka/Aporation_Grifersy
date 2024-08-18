using Game.Player;
using Game.Weapon;
using Game.SeniorEventBus;
using UnityEngine;
using Game.Data;

public class EnterMission : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Movement _movement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Raycasts _raycasts;

    [Header("Weapon")]
    [SerializeField] private WeaponAk _weaponAk;
    [SerializeField] private RPG _rpg;
    [SerializeField] private Knife _knife;
    [SerializeField] private GrenadeThrower _grenadeThrower;
    [SerializeField] private ScopeLevels _scopeLevels;
    [SerializeField] private ChangeRpgSkin _changeRpgSkin;
    [SerializeField] private ChangeAkSkin _changeAkSkin;
    [SerializeField] private ChangeKnifeSkin _changeKnifeSkin;
    [SerializeField] private ChangeWeapon _changeWeapon;

    [Header("UI")]
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GamePanel _gamePanel;
    [SerializeField] private VolumeController _volume;

    [Header("Animations")]
    [SerializeField] private AKAnim _akAnim;
    [SerializeField] private RPGAnim _rpgAnim;
    [SerializeField] private GrenadeAnim _grenadeAnim;
    [SerializeField] private KnifeAnim _knifeAnim;

    [Header("Data")]
    [SerializeField] private JSON_load_mission _loadDataJson;
    [SerializeField] private JSON_save_mission _saveDataJson;

    [SerializeField] private bool _load = true;

    private EventBus _eventBus;
    private EnemyListController _enemyListController;
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

        _saveDataJson.Init();
        _loadDataJson.Init();

        if (_load)
            _loadDataJson.Load();

        _changeWeapon.Init();
        _gamePanel.Init();
        _playerHealth.Init();
        _movement.Init();
        _weaponAk.Init();
        _rpg.Init();
        _knife.Init();
        _enemyListController.Init();
        _cameraController.Init();
        _coinSystem.Init();
        _scopeLevels.Init();
        _grenadeThrower.Init();
        _raycasts.Init();

        _akAnim.Init();
        _rpgAnim.Init();
        _grenadeAnim.Init();
        _knifeAnim.Init();

        _changeAkSkin.Init();
        _changeKnifeSkin.Init();
        _changeRpgSkin.Init();

        _gameUI.Init();

        Debug.Log("Initializated");
    }

    private void Register()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register<PlayerHealth>(_playerHealth);
        ServiceLocator.Current.Register<Movement>(_movement);
        ServiceLocator.Current.Register<ChangeWeapon>(_changeWeapon);
        ServiceLocator.Current.Register<RPG>(_rpg);
        ServiceLocator.Current.Register<WeaponAk>(_weaponAk);
        ServiceLocator.Current.Register<CoinSystem>(_coinSystem);
        ServiceLocator.Current.Register<CameraController>(_cameraController);
        ServiceLocator.Current.Register<ScopeLevels>(_scopeLevels);
        ServiceLocator.Current.Register<GrenadeThrower>(_grenadeThrower);
        ServiceLocator.Current.Register<VolumeController>(_volume);
        Debug.Log("Registreted");
    }
}
