using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Camera _uiCamera;
    private bool PauseGame;
    private GameObject _mainCharacter;
    private AudioListener _audioListener;

    public void Init()
    {
        _audioListener = _uiCamera.GetComponent<AudioListener>();
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _uiCamera.enabled = false;
        _audioListener.enabled = false;
        gamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        gamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        _uiCamera.enabled = true;
        _audioListener.enabled = true;
        _mainCharacter.SetActive(false);
    }

    public void Continue()
    {
        _mainCharacter.SetActive(true);
        _uiCamera.enabled = false;
        _audioListener.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
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

}