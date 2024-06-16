using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    public GameObject BuyButton;
    public GameObject UseButton;

    public string Key;
    public int SaveCondition;
    public int Number;
}
