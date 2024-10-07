using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine; 

public class EndMission : MonoBehaviour
{
    [SerializeField] private int _signalsNeeded = 2;
    [SerializeField] private int _index = 0;
    [SerializeField] private int _signals = 0;
    [SerializeField] private int _money = 1000;

    [SerializeField] private float _time = 1f;

    [SerializeField] private bool _saveBefore = false;

    private bool _isStarted = false;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<EndSignal>(AddSignal, 1);

        _isStarted = true;
    }

    private void AddSignal(EndSignal signal)
    {
        _signals++;
        if (_signals >= _signalsNeeded)
        {
            Invoke("EndGame", _time);
            _eventBus.Invoke(new MoneyAdd(_money));

            if (_saveBefore)
            {
                ConstSystem.CanSave = true;
                _eventBus.Invoke(new SaveDataSignal());
            }
        }
    }

    private void EndGame()
    {
        PlayerPrefsSafe.SetInt(ConstSystem.MISSION_KEY, _index);
        _eventBus.Invoke(new SetWin());
    }

    private void OnDestroy()
    {
        if (_isStarted)
            _eventBus.Unsubscribe<EndSignal>(AddSignal);
    }
}

