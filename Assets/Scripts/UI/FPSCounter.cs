using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text _fpsText;  

    private int _frameCount = 0;
    private float _deltaTime = 0.0f;
    private float _updateInterval = 1; 

    void Update()
    {
        _frameCount++;
        _deltaTime += Time.deltaTime;

        if (_deltaTime > _updateInterval)
        {
            _fpsText.text = _frameCount.ToString();

            _frameCount = 0;
            _deltaTime -= _updateInterval;
        }
    }
}