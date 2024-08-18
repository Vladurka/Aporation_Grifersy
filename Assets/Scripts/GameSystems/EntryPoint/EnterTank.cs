using UnityEngine;
using Game.SeniorEventBus;

public class EnterTank : MonoBehaviour
{
    [SerializeField] private Stryker _stryker;
    [SerializeField] private ATGM _atgm;
    [SerializeField] private Minigun _minigun;
    [SerializeField] private GameUI _gameUI;

    private EnemyListController _enemyListController;
    private EventBus _eventBus;

    private void Awake()
    {
        _enemyListController = new EnemyListController();
        _eventBus = new EventBus();

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
        _stryker.Init();    
        _atgm.Init();
        _minigun.Init();
        _gameUI.Init();
        _enemyListController.Init();
    }
    private void Register()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_enemyListController);
    }
}
