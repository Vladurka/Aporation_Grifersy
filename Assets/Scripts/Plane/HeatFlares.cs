using UnityEngine;
using UnityEngine.UI;

public class HeatFlares : MonoBehaviour
{
    [SerializeField] private GameObject _flareTrap;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Text _flrText;

    [SerializeField] private int _flrAmount = 20;

    [SerializeField] private AudioClip _flrSound;
    private AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _flrText.text = _flrAmount.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _flrAmount > 0)
        {
            Instantiate(_flareTrap, _spawnPos.position, _spawnPos.rotation);
            _flrAmount--;
            _flrText.text = _flrAmount.ToString();
            _audioSource.PlayOneShot(_flrSound);
        }  
    }
}
