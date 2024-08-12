using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour, IService
{
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

            if(loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation)
                loadAsync.allowSceneActivation = true;

            yield return null;
        }
    }

}