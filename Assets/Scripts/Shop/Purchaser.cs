using UnityEngine;

public class Purchaser : MonoBehaviour
{
    public void BuyNewAkSkin(SkinData info)
    {
        PlayerPrefsSafe.SetInt(ConstSystem.AK_SKIN_INDEX, info.Number);
        PlayerPrefsSafe.SetInt(info.Key, 1);
        info.UseButton.SetActive(true);
    }

    public void BuyNewKnifeSkin(SkinData info)
    {
        PlayerPrefsSafe.SetInt(ConstSystem.KNIFE_SKIN_INDEX, info.Number);
        PlayerPrefsSafe.SetInt(info.Key, 1);
        info.UseButton.SetActive(true);
    }

    public void BuyNewCarSkin(SkinData info)
    {
        PlayerPrefsSafe.SetInt(ConstSystem.CAR_SKIN_INDEX, info.Number);
        PlayerPrefsSafe.SetInt(info.Key, 1);
        info.UseButton.SetActive(true);
    }

    public void BuyNewRpgSkin(SkinData info)
    {
        PlayerPrefsSafe.SetInt(ConstSystem.RPG_SKIN_INDEX, info.Number);
        PlayerPrefsSafe.SetInt(info.Key, 1);
        info.UseButton.SetActive(true);
    }

    public void BuyNewGlovesSkin(SkinData info)
    {
        PlayerPrefsSafe.SetInt(ConstSystem.GLOVES_SKIN_INDEX, info.Number);
        PlayerPrefsSafe.SetInt(info.Key, 1);
        info.UseButton.SetActive(true);
    }
}
