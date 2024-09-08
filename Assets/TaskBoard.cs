using UnityEditor.Rendering;
using UnityEngine;

public class TaskBoard : MonoBehaviour, ITaskBoard
{
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _cameraUI;
    [SerializeField] private GameObject _boardPanel;
    public void Open()
    {
        _mainCharacter.SetActive(false);
        ConstSystem.IsBeasy = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        _cameraUI.SetActive(true);
        _boardPanel.SetActive(true);
    }

    public void Close()
    {
        _mainCharacter.SetActive(true);
        ConstSystem.IsBeasy = false;
        _boardPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _cameraUI.SetActive(false);
    }
}
