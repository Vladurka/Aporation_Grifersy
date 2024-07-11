using UnityEngine;

public class PlayCutScene : MonoBehaviour
{
    [SerializeField] private GameObject _videoPanel;
    public void CutScenePlay()
    {
        _videoPanel.SetActive(true);
    }
}
