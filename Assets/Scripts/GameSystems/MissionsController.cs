using UnityEngine;

public class MissionsController : MonoBehaviour
{
    [SerializeField] private int _missionCondition = 0;

    [SerializeField] private GameObject[] _missionsButtons;

    [SerializeField] private GameObject _car;
    [SerializeField] private GameObject _helicopter;
    [SerializeField] private GameObject _mission3Enemies;

    public void Init()
    {
        _car.SetActive(false);
        _helicopter.SetActive(false);

        foreach (GameObject button in _missionsButtons)
        {
            button.SetActive(false);
        }

        _missionCondition = PlayerPrefs.GetInt(ConstSystem.MISSION_KEY);

        if (_missionCondition == 0 || !PlayerPrefs.HasKey(ConstSystem.MISSION_KEY))
            _missionsButtons[0].SetActive(true);

        if (_missionCondition == 1)
        {
            _missionsButtons[0].SetActive(false);
            _missionsButtons[1].SetActive(true);
        }

        if (_missionCondition == 2)
        {
            _missionsButtons[1].SetActive(false);
            _missionsButtons[2].SetActive(true);
        }
        if (_missionCondition == 3)
        {
            _missionsButtons[3].SetActive(false);
            _missionsButtons[4].SetActive(true);
        }

        if (_missionCondition == 5)
            _missionsButtons[4].SetActive(false);

        if (_missionCondition >= 3)
        {
            _car.SetActive(true);
            _mission3Enemies.SetActive(false);
        }


        if (_missionCondition >= 4)
            _helicopter.SetActive(true);

    }
}
