using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private string[] _tips;
    [SerializeField] private Text _text;
    [SerializeField] private string _key = "Tips";

    private int _index = 0;
    private void Start()
    {
        if (!PlayerPrefs.HasKey(_key))
            _text.text = _tips[_index];
    }

    private void Update()
    {
        if (!PlayerPrefs.HasKey(_key))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_index < _tips.Length - 1)
                {
                    _index++;
                    _text.text = _tips[_index];
                }

                else if (_index >= _tips.Length - 1)
                {
                    _text.text = "";
                    PlayerPrefs.SetInt(_key, 1);
                }
            }
        }
    }
}

