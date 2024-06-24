using UnityEngine;

public class MissionsController : MonoBehaviour
{
    [SerializeField] private int _missionCondition = 0;

    [SerializeField] private GameObject[] _missionsButtons;

    public void Init()
    {
        foreach (GameObject button in _missionsButtons)
        {
            button.SetActive(false);
        }

        _missionCondition = PlayerPrefs.GetInt(ConstSystem.MISSION_KEY);

        if (_missionCondition == 0 || !PlayerPrefs.HasKey(ConstSystem.MISSION_KEY))
            _missionsButtons[0].SetActive(true);

        if (_missionCondition == 1)
        {
            _missionsButtons[1].SetActive(false);
            _missionsButtons[2].SetActive(true);
        }

        if (_missionCondition == 1)
        {
            _missionsButtons[2].SetActive(false);
            _missionsButtons[3].SetActive(true);
        }
        if (_missionCondition == 1)
        {
            _missionsButtons[3].SetActive(false);
            _missionsButtons[4].SetActive(true);
        }

        if (_missionCondition == 1)
            _missionsButtons[4].SetActive(false);

    }
}
