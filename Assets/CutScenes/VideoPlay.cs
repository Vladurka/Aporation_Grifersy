using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private void OnEnable()
    {
        _videoPlayer.Play();
        _videoPlayer.loopPointReached += ChangeScene;
    }

    private void ChangeScene(VideoPlayer player)
    {
        
    }
}
