using UnityEngine;
using UnityEngine.UI;

public class MissionsController : MonoBehaviour
{
    public static int MissionCondition;

    [SerializeField] private Button[] _missionsButtons;

    [SerializeField] private GameObject _car;

    [SerializeField] private GameObject _mission3Things;
    [SerializeField] private GameObject _mission4Things;

    public void Init()
    {
        _car.SetActive(false);
        _mission3Things.SetActive(false);
        _mission4Things.SetActive(false);

        if(MissionCondition >= 3)
            _car.SetActive(true);

        MissionCondition = PlayerPrefsSafe.GetInt(ConstSystem.MISSION_KEY);

        UpdateMissionButtons(MissionCondition);
    }

    private void SetButtonState(Button button, bool interactable, Color color)
    {
        if (button == null) return;

        button.interactable = interactable;
        Image image = button.GetComponent<Image>();

        if (image != null)
            image.color = color;
    }

    private void ResetButtonStates()
    {
        foreach (Button button in _missionsButtons)
            SetButtonState(button, false, Color.red);
    }

    private void UpdateMissionButtons(int missionCondition)
    {
        if (_missionsButtons == null || _missionsButtons.Length == 0)
        {
            Debug.LogError("The _missionsButtons array is null or empty.");
            return;
        }

        ResetButtonStates();

        if (missionCondition == 0)
            SetButtonState(_missionsButtons[0], true, Color.white);

        else if (missionCondition > 0 && missionCondition < _missionsButtons.Length)
        {
            SetButtonState(_missionsButtons[missionCondition - 1], false, Color.red);

            SetButtonState(_missionsButtons[missionCondition], true, Color.white);

            for (int i = 0; i < missionCondition; i++)
                SetButtonState(_missionsButtons[i], false, Color.green);
        }

        else
            Debug.LogError("MissionCondition out of bounds.");
    }

    public void CurrentMission(int currentIndex)
    {
        if (currentIndex == 3)
        {
            _mission3Things.SetActive(true);
            _car.SetActive(true);
            ConstSystem.CanSave = false;
        }

        if (currentIndex == 4)
        {
            _mission4Things.SetActive(true);
            ConstSystem.CanSave = false;
            ConstSystem.CanFixHelicopter = true;
        }
    }
}
