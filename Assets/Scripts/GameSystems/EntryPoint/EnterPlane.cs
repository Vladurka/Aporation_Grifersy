using UnityEngine;
using Game.SeniorEventBus;
using Game.Data;

public class EnterPlane : MonoBehaviour
{
    [SerializeField] private JSON_save_money _save;
    [SerializeField] private JSON_load_money _load;

    [SerializeField] private PlaneController _planeController;
    [SerializeField] private PlaneHealth _planeHealth;
    [SerializeField] private Minigun _minigun;
    [SerializeField] private StopDetector _stopDetector;
    [SerializeField] private GameUI _gameUI;

    private EnemyListController _enemyListController;
    private EventBus _eventBus;
    private CoinSystem _coinSystem;

    private void Awake()
    {
        _enemyListController = new EnemyListController();
        _eventBus = new EventBus();
        _coinSystem = new CoinSystem();

        Register();
        Init();
    }


    private void Init()
    {
        _load.Init();
        _load.Load();
        _save.Init();

        _coinSystem.Init();
        _planeController.Init();
        _stopDetector.Init();
        _minigun.Init();
        _planeHealth.Init();
        _gameUI.Init();
        _enemyListController.Init();
    }
    private void Register()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_enemyListController);
        ServiceLocator.Current.Register(_coinSystem);
        ServiceLocator.Current.Register<PlaneController>(_planeController);
        ServiceLocator.Current.Register<StopDetector>(_stopDetector);
    }
}
