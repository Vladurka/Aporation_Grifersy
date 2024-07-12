using UnityEngine;

public class MissionsController : MonoBehaviour
{
    public static int MissionCondition = 3;

    [SerializeField] private GameObject[] _missionsButtons;

    [SerializeField] private GameObject _car;

    [SerializeField] private GameObject _mission3Enemies;
    [SerializeField] private GameObject _mission4Enemies;

    public void Init()
    {
        _car.SetActive(false);
        _mission3Enemies.SetActive(false);
        _mission4Enemies.SetActive(false);

        foreach (GameObject button in _missionsButtons)
        {
            button.SetActive(false);
        }

        MissionCondition = PlayerPrefs.GetInt(ConstSystem.MISSION_KEY);

        if (MissionCondition == 1)
        {
            _missionsButtons[0].SetActive(false);
            _missionsButtons[1].SetActive(true);
        }

        if (MissionCondition == 2)
        {
            _missionsButtons[1].SetActive(false);
            _missionsButtons[2].SetActive(true);
        }
        if (MissionCondition == 3)
        {
            _missionsButtons[2].SetActive(false);
            _missionsButtons[3].SetActive(true);
        }

        if (MissionCondition == 4)
        {
            _missionsButtons[3].SetActive(false);
            _missionsButtons[4].SetActive(true);
        }

        if (MissionCondition == 5)
            _missionsButtons[4].SetActive(false);


        if (MissionCondition >= 3)
            _car.SetActive(true);

    }

    public void CurrentMission(int currentIndex)
    {
        if(currentIndex == 3)
            _mission3Enemies.SetActive(true);

        if(currentIndex == 4)
            _mission4Enemies.SetActive(true);

        if(currentIndex != 3)
            _mission3Enemies.SetActive(false);

        if (currentIndex != 4)
            _mission4Enemies.SetActive(false);
    }
}
