using UnityEngine;
using UnityEngine.Video;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private GameObject[] _panels;
    [SerializeField] private GameObject _videoPanel;
    [SerializeField] private Loading _loading;
    [SerializeField] private GameObject _uiCamera;

    private GameObject _mainCharacter;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<PlayCut>(PlayCut, 1);

        if(_loading == null)
            _loading = ServiceLocator.Current.Get<Loading>();

        _mainCharacter = GameObject.FindGameObjectWithTag("Player");

        Debug.Log(_loading);
    }

    public void PlayVideo(int index)
    {
        Cursor.lockState = CursorLockMode.Locked;
        ConstSystem.CanPause = false;

        _videoPanel.SetActive(true);
        _video.Play();
        _video.loopPointReached += (vp) => Load(vp, index);

        _uiCamera.SetActive(true);
        _mainCharacter.SetActive(false);
    }

    private void PlayCut(PlayCut cut)
    {
        PlayVideo(cut.Index);
        Debug.Log("Play");
    }

    private void Load(VideoPlayer vp, int index)
    {
        foreach (GameObject panel in _panels)
            panel.SetActive(false);

        _loading.StartLoading(index);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<PlayCut>(PlayCut);
    }
}
