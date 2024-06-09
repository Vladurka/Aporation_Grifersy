using UnityEngine;

public class SkinInfo : MonoBehaviour
{
    [SerializeField] private string _key;

    private int _saveCondition;

    public int Number;

    public GameObject BuyButton;
    public GameObject UseButton;

    private void Start()
    {
        _saveCondition = PlayerPrefs.GetInt(_key);

        if (_saveCondition == 1)
            CanUse();

        if (!PlayerPrefs.HasKey(_key))
            CantUse();
    }

    public void Use()
    {
        Invoke("CanUse", 0.1f);
    }

    private void CanUse()
    {
        BuyButton.SetActive(false);
        UseButton.SetActive(true);
        PlayerPrefs.SetInt(_key, 1);
        PlayerPrefs.Save();
    }

    private void CantUse()
    {
        BuyButton.SetActive(true);
        UseButton.SetActive(false);
        PlayerPrefs.SetInt(_key, 0);
        PlayerPrefs.Save();
    }
}
