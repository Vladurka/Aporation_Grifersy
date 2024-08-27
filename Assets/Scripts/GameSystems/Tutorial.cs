using UnityEngine;
using UnityEngine.UI;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private string[] _tips;
    [SerializeField] private Text _text;
    [SerializeField] private string _key = "Tips";
    [SerializeField] private bool _repeat = true;

    private EventBus _eventBus;

    private int _index = 0;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<NextTip>(Next, 1);

        if (!_repeat)
        {
            if (!PlayerPrefsSafe.HasKey(_key))
                _text.text = _tips[_index];
        }

        else if (_repeat)
            _text.text = _tips[_index];

        else
            _text.text = "";
    }

    private void Next(NextTip tip)
    {
        if (!_repeat)
        {
            if (!PlayerPrefsSafe.HasKey(_key))
            {
                if (_index < _tips.Length - 1)
                {
                    _index++;
                    _text.text = _tips[_index];
                }

                else if (_index >= _tips.Length - 1)
                {
                    _text.text = "";
                    PlayerPrefsSafe.SetInt(_key, 1);
                }
            }
        }

        if (_repeat)
        {
            if (_index < _tips.Length - 1)
            {
                _index++;
                _text.text = _tips[_index];
            }

            else if (_index >= _tips.Length - 1)
            {
                _text.text = "";
                PlayerPrefsSafe.SetInt(_key, 1);
            }
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<NextTip>(Next);
    }
}

