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

    private bool _pauseGame = false;
    private bool _isStarted = false;
    private bool _isDead = false;
    private bool _isCompleted = false;

    private EventBus _eventBus;
    public void Init()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetDie>(SetDie, 1);
        _eventBus.Subscribe<SetWin>(SetWin, 1);
        _eventBus.Subscribe<OpenShop>(OpenShop, 1);
        _eventBus.Subscribe<CloseShop>(CloseShop);
        _eventBus.Subscribe<OpenBoard>(OpenBoard, 1);
        _eventBus.Subscribe<CloseBoard>(CloseBoard, 1);

        ConstSystem.CanPause = true;
        ConstSystem.InCar = false;
        ConstSystem.InTransport = false;
        ConstSystem.CanSave = true;
        ConstSystem.InDrone = false;

        if (_shopPanel != null)
            _shopPanel.SetActive(false);

        if (_completedPanel != null)
            _completedPanel.SetActive(false);

        if(_levelsPanel != null)
            _levelsPanel.SetActive(false);

        _uiCamera.SetActive(false);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _diePanel.SetActive(false);

        _gamePanel.SetActive(true);

        _isStarted = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseGame && !_isDead && !_isCompleted)
                Continue();

            else if (!_pauseGame && !_isDead && !_isCompleted)
                Pause();
        }
    }

    #region Pause
    public void Pause()
    {
        if (ConstSystem.CanPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;

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

            _pauseGame = true;
        }
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        if (!ConstSystem.InTransport)
        {
            _mainCharacter.SetActive(true);
            _gamePanel.SetActive(true);
        }

        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _uiCamera.SetActive(false);

        if (_shopPanel != null && _levelsPanel != null)
        {
            _shopPanel.SetActive(false);
            _levelsPanel.SetActive(false);
        }

        _eventBus.Invoke(new SetSpeedometer(false));
        _eventBus.Invoke(new SetDronePanel(false));

        if (ConstSystem.InCar)
            _eventBus.Invoke(new SetSpeedometer(true));

        if (ConstSystem.InDrone)
            _eventBus.Invoke(new SetDronePanel(true));

        ConstSystem.CanExit = true;

        _pauseGame = false;
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
    #endregion

    #region Shop
    private void OpenShop(OpenShop open)
    {
        _eventBus.Invoke(new SaveDataSignal());

        Cursor.lockState = CursorLockMode.None;
        ConstSystem.IsBeasy = true;
        ConstSystem.CanPause = false;
        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true);
        _shopPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    private void CloseShop(CloseShop close)
    {
        Time.timeScale = 1f;
        ConstSystem.IsBeasy = false;
        ConstSystem.CanPause = true;
        _shopPanel.SetActive(false);
        _mainCharacter.SetActive(true);
        _uiCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region TaskBoad
    private void OpenBoard(OpenBoard open)
    {
        _eventBus.Invoke(new SaveDataSignal());

        Cursor.lockState = CursorLockMode.None;
        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true);
        _levelsPanel.SetActive(true);
        ConstSystem.IsBeasy = true;
        ConstSystem.CanPause = false;
        Time.timeScale = 0f;
    }

    private void CloseBoard(CloseBoard board)
    {
        Cursor.lockState = CursorLockMode.Locked;
        _levelsPanel.SetActive(false);
        _uiCamera.SetActive(false);
        _mainCharacter.SetActive(true);
        ConstSystem.IsBeasy = false;
        ConstSystem.CanPause = true;
        Time.timeScale = 1f;
    }
    #endregion
    private void SetDie(SetDie set)
    {
        Cursor.lockState = CursorLockMode.None;

        ConstSystem.CanSave = false;

        _uiCamera.SetActive(true);
        _diePanel.SetActive(true);

        if (_mainCharacter != null)
            _mainCharacter.SetActive(false);

        _eventBus.Invoke(new SetSpeedometer(false));
        _eventBus.Invoke(new SetDronePanel(false));
        Time.timeScale = 0f;

        _isDead = true;
    }

    private void SetWin(SetWin set)
    {
        Cursor.lockState = CursorLockMode.None;
        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true);
        _completedPanel.SetActive(true);
        ConstSystem.CanPause = false;
        _eventBus.Invoke(new SetDronePanel(false));
        _eventBus.Invoke(new SetSpeedometer(false));
        Time.timeScale = 0f;

        _isCompleted = true;
    }

    private void OnDestroy()
    {
        if (_isStarted)
        {
            _eventBus.Unsubscribe<SetDie>(SetDie);
            _eventBus.Unsubscribe<SetWin>(SetWin);
            _eventBus.Unsubscribe<OpenShop>(OpenShop);
            _eventBus.Unsubscribe<CloseShop>(CloseShop);
            _eventBus.Unsubscribe<OpenBoard>(OpenBoard);
            _eventBus.Unsubscribe<CloseBoard>(CloseBoard);
        }
    }
}