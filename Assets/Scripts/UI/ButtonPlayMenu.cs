using UnityEngine;

public class ButtonPlayMenu : MonoBehaviour
{
    [SerializeField] private Loading _load;
    [SerializeField] private GameObject _trainingPanel;
    private int _index = 0;
    private void Start()
    {
        if (PlayerPrefsSafe.HasKey(ConstSystem.PRISON_ENDED))
            _index = PlayerPrefsSafe.GetInt(ConstSystem.PRISON_ENDED);
    }
    public void PlayAndCheck()
    {
        if (!PlayerPrefs.HasKey(ConstSystem.STARTED_TO_PLAY))
            _trainingPanel.SetActive(true);

        else
        {
            if (!PlayerPrefsSafe.HasKey(ConstSystem.PRISON_ENDED))
                _load.StartLoading(1);

            if (_index == 1)
                _load.StartLoading(2);
        }
    }
    public void Play()
    {
        if (!PlayerPrefsSafe.HasKey(ConstSystem.PRISON_ENDED))
            _load.StartLoading(1);

        if (_index == 1)
            _load.StartLoading(2);
    }
}
