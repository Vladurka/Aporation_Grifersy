using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation _asyncOperation;
    [SerializeField] private Image _loadBar;
    [SerializeField] private Text _barTxt;
    [SerializeField] private int _sceneID;

    public void Load()
    {
        StartCoroutine(LoadSceneCor());
    }

    private IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        _asyncOperation = SceneManager.LoadSceneAsync(_sceneID);
        while (!_asyncOperation.isDone)
        {
            float progress = _asyncOperation.progress / 0.9f;
            _loadBar.fillAmount = progress;
            _barTxt.text = "Loading Complete   " + string.Format("{0:0}%", progress * 100f);
            yield return 0;
        }
    }

}