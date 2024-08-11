using UnityEngine;
using UnityEngine.Playables;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    private Loading _loading;

    private void Start()
    {
        _loading = ServiceLocator.Current.Get<Loading>();
    }

    public void PlayCutscene(PlayableDirector playableDirector)
    {
        //_playableDirector.stopped += (director) => OnPlayableDirectorStopped(index);
        playableDirector.Play();
    }

    private void OnPlayableDirectorStopped(int index)
    {
        _loading.StartLoading(index);
    }
}
