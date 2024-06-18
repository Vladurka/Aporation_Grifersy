using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public Image LoadBar;
    public Text BarTxt;
    public int SceneID;

    private void Start()
    {
        StartCoroutine(LoadSceneCor());
    }


    IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        asyncOperation = SceneManager.LoadSceneAsync(SceneID);
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            LoadBar.fillAmount = progress;
            BarTxt.text = "Loading Complete   " + string.Format("{0:0}%", progress * 100f);
            yield return 0;
        }
    }

}