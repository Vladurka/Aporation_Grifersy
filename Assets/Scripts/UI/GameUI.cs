using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    private bool PauseGame;

    public void Init()
    {
        Time.timeScale = 1.0f;
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
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        PauseGame = true;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gamePanel.SetActive(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
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