using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour, IService
{
    [SerializeField] private string _volumeKey = "GameVolume";
    [SerializeField] private AudioSource[] _audioSourceItems;

    [SerializeField] private GameObject _capitalObject;
    private AudioSource[] _capitalAudioSource;

    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        _capitalAudioSource = _capitalObject.GetComponentsInChildren<AudioSource>(true);

        if (PlayerPrefsSafe.HasKey(_volumeKey))
            _volumeSlider.value = PlayerPrefsSafe.GetFloat(_volumeKey);

        if (!PlayerPrefsSafe.HasKey(_volumeKey))
            _volumeSlider.value = 0.3f;

        foreach (AudioSource source in _capitalAudioSource)
            source.spatialBlend = 0.4f;

        foreach (AudioSource source in _audioSourceItems)
            source.spatialBlend = 0.4f;
    }

    private void Update()
    {
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        foreach (AudioSource audioSource in _audioSourceItems)
        {
            if(audioSource != null)
                audioSource.volume = _volumeSlider.value;
        }

        foreach (AudioSource audioSource in _capitalAudioSource)
        {
            if (audioSource != null)
                audioSource.volume = _volumeSlider.value;
        }
    }

    private void OnDisable()
    {
        PlayerPrefsSafe.SetFloat(_volumeKey, _volumeSlider.value);
    }

}