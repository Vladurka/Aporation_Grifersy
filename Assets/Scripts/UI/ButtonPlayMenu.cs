using UnityEngine;

public class ButtonPlayMenu : MonoBehaviour
{
    [SerializeField] private Loading _load;
    private int _index = 0;
    private void Start()
    {
        if (PlayerPrefsSafe.HasKey("Prison"))
            _index = PlayerPrefsSafe.GetInt("Prison");
    }
    public void Play()
    {
        if (!PlayerPrefsSafe.HasKey("Prison"))
            _load.StartLoading(1);

        if (_index == 1)
            _load.StartLoading(2);

        Debug.Log(_index);
    }
}
