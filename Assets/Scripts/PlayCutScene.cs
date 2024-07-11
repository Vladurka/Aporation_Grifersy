using UnityEngine;
using UnityEngine.Playables;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _cutCamera;
    [SerializeField] private Camera _cameraUI;
    [SerializeField] private Canvas _canvas;
    public void PlayScene(PlayableDirector _cutScene)
    {
        _mainCamera.enabled = false;
        _cutCamera.enabled = true;
        _cameraUI.enabled = false;
        _canvas.enabled = false;
        _cutScene.Play();
    }
}
