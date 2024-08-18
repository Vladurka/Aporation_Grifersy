using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour, IService
{
    [SerializeField] private string _volumeKey = "GameVolume";
    [SerializeField] private AudioSource[] _audioSource;

    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey(_volumeKey))
            _volumeSlider.value = PlayerPrefsSafe.GetFloat(_volumeKey);

        else
            _volumeSlider.value = 0.3f;
    }

    private void Update()
    {
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        foreach (AudioSource audioSource in _audioSource)
        {
            if (audioSource != null)
                audioSource.volume = _volumeSlider.value;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefsSafe.SetFloat(_volumeKey, _volumeSlider.value);
    }

}