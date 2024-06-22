using UnityEngine;

public class MissionsController : MonoBehaviour
{
    private int _mission1Condition = 0;
    private int _mission2Condition = 0;
    private int _mission3Condition = 0;
    private int _mission4Condition = 0;
    private int _mission5Condition = 0;

    [SerializeField] private GameObject[] _missionsButtons;

    public void Init()
    {
        foreach (GameObject button in _missionsButtons)
        {
            button.SetActive(true);
        }

        //_mission1Condition = PlayerPrefs.GetInt(ConstSystem.MISSION1_KEY);
        //if (_mission1Condition == 1)
        //{
        //    _missionsButtons[0].SetActive(false);
        //    _missionsButtons[1].SetActive(true);
        //}

        //_mission2Condition = PlayerPrefs.GetInt(ConstSystem.MISSION2_KEY);
        //if (_mission2Condition == 1)
        //{
        //    _missionsButtons[1].SetActive(false);
        //    _missionsButtons[2].SetActive(true);
        //}

        //_mission3Condition = PlayerPrefs.GetInt(ConstSystem.MISSION3_KEY);
        //if (_mission3Condition == 1)
        //{
        //    _missionsButtons[2].SetActive(false);
        //    _missionsButtons[3].SetActive(true);
        //}

        //_mission4Condition = PlayerPrefs.GetInt(ConstSystem.MISSION4_KEY);
        //if (_mission4Condition == 1)
        //{
        //    _missionsButtons[3].SetActive(false);
        //    _missionsButtons[4].SetActive(true);
        //}

        //_mission5Condition = PlayerPrefs.GetInt(ConstSystem.MISSION5_KEY);
        //if (_mission5Condition == 1)
        //    _missionsButtons[4].SetActive(false);

    }
}
