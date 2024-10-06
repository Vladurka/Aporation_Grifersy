using UnityEngine;

public class EnterMenu : MonoBehaviour
{
    [SerializeField] private ChangeAkSkin _akSkin;
    [SerializeField] private ChangeRpgSkin _rpgSkin;
    [SerializeField] private ChangeSkinCar _skinCar;
    [SerializeField] private ChangeKnifeSkin _knifeSkin;

    private void Awake()
    {
        Time.timeScale = 1f;

        _akSkin.Init();
        _rpgSkin.Init();
        _knifeSkin.Init();
        _skinCar.Init();
    }
}
