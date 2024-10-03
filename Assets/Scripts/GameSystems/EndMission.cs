using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine; 

public class EndMission : MonoBehaviour
{
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _uiCamera;
    [SerializeField] private int _signalsNeeded = 2;
    [SerializeField] private int _index = 0;
    [SerializeField] private float _time = 1f;
    [SerializeField] private int _signals = 0;
    [SerializeField] private int _money = 1000;

    private bool _isStarted = false;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<EndSignal>(AddSignal, 1);

        _isStarted = true;
    }

    private void AddSignal(EndSignal signal)
    {
        _signals++;
        if (_signals >= _signalsNeeded)
        {
            Invoke("EndGame", _time);
            _eventBus.Invoke(new MoneyAdd(_money));
        }
    }

    private void EndGame()
    {
        PlayerPrefsSafe.SetInt(ConstSystem.MISSION_KEY, _index);
        Cursor.lockState = CursorLockMode.None;
        _uiCamera.SetActive(true); 
        _endPanel.SetActive(true);
        ConstSystem.CanPause = false;
        _eventBus.Invoke(new SetDronePanel(false));
        _eventBus.Invoke(new SetSpeedometer(false));
        _mainCharacter.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        if (_isStarted)
            _eventBus.Unsubscribe<EndSignal>(AddSignal);
    }
}

