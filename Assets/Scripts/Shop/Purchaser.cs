using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    public void OnPurchaseComplete(Product product)
    {
        switch (product.definition.id)
        {
            case ConstSystem.BUY_AK_SKIN_ID:
                break;

            case ConstSystem.BUY_KNIFE_SKIN_ID:
                break;

            case ConstSystem.BUY_CAR_SKIN_ID:
                break;

            case ConstSystem.BUY_RPG_SKIN_ID:
                break;
        }
    }
    public void BuyNewAkSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.AK_SKIN_INDEX, info.Number);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewKnifeSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.KNIFE_SKIN_INDEX, info.Number);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewCarSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.CAR_SKIN_INDEX, info.Number);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }

    public void BuyNewRpgSkin(SkinData info)
    {
        PlayerPrefs.SetInt(ConstSystem.RPG_SKIN_INDEX, info.Number);
        PlayerPrefs.Save();
        info.UseButton.SetActive(true);
    }
}
