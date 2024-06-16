using UnityEngine;

public class Purchaser : MonoBehaviour
{
    public void BuyNewAkSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.AK_SKIN_INDEX, info.Number);
        PlayerPrefs.SetInt(info.Key, 1);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewKnifeSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.KNIFE_SKIN_INDEX, info.Number);
        PlayerPrefs.SetInt(info.Key, 1);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewCarSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.CAR_SKIN_INDEX, info.Number);
        PlayerPrefs.SetInt(info.Key, 1);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewRpgSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.RPG_SKIN_INDEX, info.Number);
        PlayerPrefs.SetInt(info.Key, 1);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewGlovesSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.GLOVES_SKIN_INDEX, info.Number);
        PlayerPrefs.SetInt(info.Key, 1);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }
}
