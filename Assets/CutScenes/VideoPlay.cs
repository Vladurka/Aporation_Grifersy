using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    private void OnEnable()
    {
        _videoPlayer.Play();
    }
}
