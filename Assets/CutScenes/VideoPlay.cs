using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _videoPlayer.Play();
        _videoPlayer.loopPointReached += ChangeScene;
    }

    private void ChangeScene(VideoPlayer player)
    {
        
    }
}
