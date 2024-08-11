using UnityEngine;
using UnityEngine.Video;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private VideoPlayer _video;
    [SerializeField] private GameObject[] _panels;
    [SerializeField] private GameObject _videoPanel;
    private Loading _loading;

    private void Start()
    {
        _loading = ServiceLocator.Current.Get<Loading>();
        Debug.Log(_loading);
    }

    public void PlayVideo(int index)
    {
        Cursor.lockState = CursorLockMode.Locked;
        ConstSystem.CanPause = false;

        _videoPanel.SetActive(true);
        _video.Play();
        _video.loopPointReached += (vp) => Load(vp, index);
    }

    private void Load(VideoPlayer vp, int index)
    {
        foreach (GameObject panel in _panels)
            panel.SetActive(false);

        _loading.StartLoading(index);
    }
}
