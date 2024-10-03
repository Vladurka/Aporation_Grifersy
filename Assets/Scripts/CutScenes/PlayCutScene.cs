using UnityEngine;
using UnityEngine.Video;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private VideoPlayer _video2;
    [SerializeField] private GameObject[] _panels;
    [SerializeField] private GameObject _videoPanel;
    [SerializeField] private GameObject _videoPanel2;
    [SerializeField] private Loading _loading;
    [SerializeField] private GameObject _uiCamera;

    private GameObject _mainCharacter;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<PlayCut>(PlayCut, 1);
        _eventBus.Subscribe<PlayLast>(PlayLastVideo);

        if(_loading == null)
            _loading = ServiceLocator.Current.Get<Loading>();

        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    public void PlayVideo(int index)
    {
        Cursor.lockState = CursorLockMode.Locked;
        ConstSystem.CanPause = false;

        _videoPanel.SetActive(true);
        _video.Play();
        _video.loopPointReached += (vp) => Load(vp, index);

        _mainCharacter.SetActive(false);
        _uiCamera.SetActive(true);
    }

    private void PlayLastVideo(PlayLast last)
    {
        Cursor.lockState = CursorLockMode.Locked;
        ConstSystem.CanPause = false;

        _videoPanel2.SetActive(true);
        _video2.Play();
        _video2.loopPointReached += (vp) => Load(vp, 0);

        _mainCharacter.SetActive(false);
    }

    private void PlayCut(PlayCut cut)
    {
        PlayVideo(cut.Index);
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
        _eventBus.Unsubscribe<PlayLast>(PlayLastVideo);
    }
}
