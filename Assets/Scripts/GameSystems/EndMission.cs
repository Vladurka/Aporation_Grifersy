using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine; 

public class EndMission : MonoBehaviour
{
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _uiCamera;
    [SerializeField] private int _signalsNeeded = 2;

    private int _signals = 0;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<EndSignal>(AddSignal, 1);
    }

    private void AddSignal(EndSignal signal)
    {
        _signals++;
        if (_signals == _signalsNeeded)
            Invoke("EndGame", 1f);
    }

    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true); 
        _endPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<EndSignal>(AddSignal);
    }
}

