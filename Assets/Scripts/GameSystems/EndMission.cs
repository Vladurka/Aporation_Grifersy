using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine; 

public class EndMission : MonoBehaviour
{
    [SerializeField] private string _key;

    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _uiCamera;
    [SerializeField] private int _signalsNeeded = 2;

    [SerializeField]  private int _signals = 0;

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
        PlayerPrefs.SetInt(_key, 1);
        Cursor.lockState = CursorLockMode.None;
        _uiCamera.SetActive(true); 
        _endPanel.SetActive(true);
        _eventBus.Invoke(new EnablePause(false));
        _mainCharacter.SetActive(false);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<EndSignal>(AddSignal);
    }
}

