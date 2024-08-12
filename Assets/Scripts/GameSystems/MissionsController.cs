using UnityEngine;

public class MissionsController : MonoBehaviour
{
    public static int MissionCondition = 3;

    [SerializeField] private GameObject[] _missionsButtons;

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

        foreach (GameObject button in _missionsButtons)
            button.SetActive(false);

        MissionCondition = PlayerPrefs.GetInt(ConstSystem.MISSION_KEY);

        if (MissionCondition == 0)
            _missionsButtons[0].SetActive(true);

        if (MissionCondition > 0)
        {
            _missionsButtons[MissionCondition - 1].SetActive(false);
            _missionsButtons[MissionCondition].SetActive(true);
        }
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
