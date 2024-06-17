using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour, IService
{
    [SerializeField] private AudioSource[] _audioSource;
    public Slider _volumeSlider;

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