using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _uiCamera;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _speedometerPanel;
    [SerializeField] private GameObject _completedPanel;
    [SerializeField] private GameObject _diePanel;
    [SerializeField] private GameObject _levelsPanel;

    [SerializeField] private AudioSource _audioSource;

    private bool _pauseGame;
    private bool _canPause = true;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<EnablePause>(PauseState, 1);
        _eventBus.Subscribe<SetDie>(SetDie, 1);

        if (_shopPanel != null && _speedometerPanel != null)
        {
            _speedometerPanel.SetActive(false);
            _shopPanel.SetActive(false);
        }

        if(_completedPanel != null)
            _completedPanel.SetActive(false);

        _uiCamera.SetActive(false);
        Time.timeScale = 1.0f;
        _gamePanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
        ConstSystem.InTransport = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_canPause)
            {
                if (_pauseGame && !_diePanel.activeSelf && !_completedPanel.activeSelf)
                {
                    Continue();
                }
            }

            if (!_diePanel.activeSelf && !_completedPanel.activeSelf)
            {
                Pause();
            }
        }
    }

    private void PauseState(EnablePause pause)
    {
        _canPause = pause.State;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        _pauseGame = true;
        _gamePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true);
        _pausePanel.SetActive(true);

        if (_shopPanel != null && _speedometerPanel != null)
        {
            _speedometerPanel.SetActive(false);
            _shopPanel.SetActive(false);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _gamePanel.SetActive(true);

        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _pauseGame = false;
        _uiCamera.SetActive(false);

        if(_shopPanel != null && _levelsPanel != null)
        {
            _shopPanel.SetActive(false);
            _levelsPanel.SetActive(false);
        }

        if (!ConstSystem.InTransport)
            _mainCharacter.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackGame()
    {
        SceneManager.LoadScene("Game");
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

    public void SetLowSettings()
    {
        QualitySettings.SetQualityLevel(0, true);
    }

    public void SetMediumSettings()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    public void SetHighSettings()
    {
        QualitySettings.SetQualityLevel(5, true);
    }

    public void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;
    }

    public void WindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }

    private void SetDie(SetDie set)
    {
        _uiCamera.SetActive(true);
        _diePanel.SetActive(true);
        if(_speedometerPanel != null)
            _speedometerPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<EnablePause>(PauseState);
        _eventBus.Unsubscribe<SetDie>(SetDie);
    }
}