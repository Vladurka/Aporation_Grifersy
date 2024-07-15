using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text _fpsText;  

    private int _frameCount = 0;
    private float _deltaTime = 0.0f;
    private float _fps = 0.0f;
    private float _updateInterval = 1.0f; 

    void Update()
    {
        _frameCount++;
        _deltaTime += Time.deltaTime;

        if (_deltaTime > _updateInterval)
        {
            _fps = _frameCount / _deltaTime;
            _fpsText.text = _frameCount.ToString();

            _frameCount = 0;
            _deltaTime -= _updateInterval;
        }
    }
}