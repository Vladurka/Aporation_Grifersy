using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    //private AsyncOperation _asyncOperation;
    //[SerializeField] private Image _loadBar;
    //[SerializeField] private Text _barTxt;
    //[SerializeField] private int _sceneID;

    //public void Load()
    //{
    //    StartCoroutine(LoadSceneCor());
    //}

    //private IEnumerator LoadSceneCor()
    //{
    //    yield return new WaitForSeconds(1f);
    //    _asyncOperation = SceneManager.LoadSceneAsync(_sceneID);
    //    while (!_asyncOperation.isDone)
    //    {
    //        float progress = _asyncOperation.progress / 0.9f;
    //        _loadBar.fillAmount = progress;
    //        _barTxt.text = "Loading Complete   " + string.Format("{0:0}%", progress * 100f);
    //        yield return 0;
    //    }
    //}

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Image _loadBar;
    [SerializeField] private Text _barTxt;

    public void StartLoading(int sceneIndex)
    {
        _loadingScreen.SetActive(true);
        StartCoroutine(Load(sceneIndex));
    }

    private IEnumerator Load(int sceneIndex)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneIndex);
        loadAsync.allowSceneActivation = false;

        while (!loadAsync.isDone)
        {
            float progress = loadAsync.progress / 0.9f;
            _loadBar.fillAmount = progress;
            _barTxt.text = "Loading   " + string.Format("{0:0}%", progress * 100f);

            if(loadAsync.progress >= .9f && !loadAsync.allowSceneActivation)
                loadAsync.allowSceneActivation = true;

            yield return null;
        }
    }

}