using UnityEngine;
using UnityEngine.UI;

public class DynamicResolution : MonoBehaviour
{
    [SerializeField] private Text screenText;
    [SerializeField] private float CurScale = 1;
    [SerializeField] private float MinScale = 0.5f;
    [SerializeField] private float ScaleStep = 0.05f;
    [SerializeField] private int MinFPS = 60;
    [SerializeField] private int MaxFPS = 144;
    [SerializeField] private float Delay = 0;

    private float MinFPSS;
    private float MaxFPSS;
    private float DelayTime;

    private Vector2 MainRes;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        MainRes = new Vector2(Screen.width, Screen.height);
        DelayTime = Delay;
        MinFPSS = 1f / (float)MinFPS;
        MaxFPSS = 1f / (float)MaxFPS;
    }

    void Update()
    {
        if (Time.time > DelayTime)
        {
            if (Time.deltaTime > MinFPSS)
            {
                if (CurScale > MinScale)
                {
                    CurScale -= ScaleStep;
                    Screen.SetResolution((int)(MainRes.x * CurScale), (int)(MainRes.y * CurScale), true);
                    DelayTime = Time.time + Delay;
                }
            }
            else
            if (CurScale < 1 && Time.deltaTime < MaxFPSS)
            {
                CurScale += ScaleStep;
                Screen.SetResolution((int)(MainRes.x * CurScale), (int)(MainRes.y * CurScale), true);
                DelayTime = Time.time + Delay;
            }
            DelayTime = Time.time + 0.5f;
        }

        screenText.text = "X " + Screen.width + " / Y " + Screen.height + " / scale " + CurScale + " / " + Time.deltaTime;
    }
}