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
        gamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
    }

    public void Continue()
    {
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
}