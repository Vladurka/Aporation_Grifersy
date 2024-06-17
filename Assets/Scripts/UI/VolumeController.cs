using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioSource[] _audioSource;

    private void Start()
    {
        _volumeSlider.value = 0.1f;
    }

    private void Update()
    {
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        foreach (AudioSource audioSource in _audioSource)
        {
            audioSource.volume = _volumeSlider.value;
        }
    }

}