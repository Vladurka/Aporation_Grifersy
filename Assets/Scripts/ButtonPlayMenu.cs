using UnityEngine;

public class ButtonPlayMenu : MonoBehaviour
{
    [SerializeField] private Loading _load;
    private int _index = 0;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Prison"))
            _index = PlayerPrefs.GetInt("Prison");
    }
    public void Play()
    {
        if (!PlayerPrefs.HasKey("Prison"))
            _load.StartLoading(1);

        if (_index == 1)
            _load.StartLoading(2);

        Debug.Log(_index);
    }
}
