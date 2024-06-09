using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    private int AkSkinIndex;
    private int KnifeSkinIndex;
    private int CarSkinIndex;
    private int RpgSkinIndex;
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
    public void BuyNewAkSkin(SkinInfo info)
    {
        AkSkinIndex = info.Number;
        PlayerPrefs.SetInt(ConstSystem.AK_SKIN_INDEX, AkSkinIndex);
        PlayerPrefs.Save();
        info.Use();
    }

    public void BuyNewKnifeSkin(SkinInfo info)
    {
        KnifeSkinIndex = info.Number;
        PlayerPrefs.SetInt(ConstSystem.KNIFE_SKIN_INDEX, KnifeSkinIndex);
        PlayerPrefs.Save();
        info.Use();
    }

    public void BuyNewCarSkin(SkinInfo info)
    {
        CarSkinIndex = info.Number;
        PlayerPrefs.SetInt(ConstSystem.CAR_SKIN_INDEX, CarSkinIndex);
        PlayerPrefs.Save();
        info.Use();
    }

    public void BuyNewRpgSkin(SkinInfo info)
    {
        RpgSkinIndex = info.Number;
        PlayerPrefs.SetInt(ConstSystem.RPG_SKIN_INDEX, RpgSkinIndex);
        PlayerPrefs.Save();
        info.Use();
    }
}
