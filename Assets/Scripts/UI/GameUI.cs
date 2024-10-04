using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _uiCamera;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _diePanel;
    [SerializeField] private GameObject _levelsPanel;

    private bool _pauseGame;
    private bool _isStarted = false;

    private EventBus _eventBus;
    public void Init()
    {
        ConstSystem.CanPause = true;
        ConstSystem.InCar = false;
        ConstSystem.InTransport = false;
        ConstSystem.CanSave = true;
        ConstSystem.InDrone = false;
        _isStarted = true;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetDie>(SetDie, 1);

        if (_shopPanel != null)
            _shopPanel.SetActive(false);

        if (_completedPanel != null)
            _completedPanel.SetActive(false);

        _uiCamera.SetActive(false);
        Time.timeScale = 1.0f;
        _gamePanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseGame && !_diePanel.activeSelf && !_completedPanel.activeSelf)
                Continue();

            if (!_diePanel.activeSelf && !_completedPanel.activeSelf)
                Pause();
        }
    }

    public void Pause()
    {
        if (ConstSystem.CanPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;

            _pauseGame = true;
            _gamePanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _mainCharacter.SetActive(false);
            _uiCamera.SetActive(true);
            _pausePanel.SetActive(true);
            ConstSystem.CanExit = false;
            _eventBus.Invoke(new SetSpeedometer(false));
            _eventBus.Invoke(new SetDronePanel(false));

            if (_shopPanel != null)
                _shopPanel.SetActive(false);
        }
    }

    public void Continue()
    {
        if (ConstSystem.CanPause)
        {
            Time.timeScale = 1.0f;
            _settingsPanel.SetActive(false);
            _pausePanel.SetActive(false);
            _pauseGame = false;
            _uiCamera.SetActive(false);
            _eventBus.Invoke(new SetSpeedometer(false));
            _eventBus.Invoke(new SetDronePanel(false));
            ConstSystem.CanExit = true;

            if (_shopPanel != null && _levelsPanel != null)
            {
                _shopPanel.SetActive(false);
                _levelsPanel.SetActive(false);
            }

            if (!ConstSystem.InTransport)
            {
                _mainCharacter.SetActive(true);
                _gamePanel.SetActive(true);
            }

            if (ConstSystem.InCar)
               _eventBus.Invoke(new SetSpeedometer(true));

            if (ConstSystem.InDrone)
                _eventBus.Invoke(new SetDronePanel(true));

            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void Settings()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    private void SetDie(SetDie set)
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        _uiCamera.SetActive(true);
        _diePanel.SetActive(true);
        ConstSystem.CanSave = false;

        if (_mainCharacter != null)
            _mainCharacter.SetActive(false);

        _eventBus.Invoke(new SetSpeedometer(false));
        _eventBus.Invoke(new SetDronePanel(false));
    }

    private void OnDestroy()
    {
        if (_isStarted)
            _eventBus.Unsubscribe<SetDie>(SetDie);
    }
}