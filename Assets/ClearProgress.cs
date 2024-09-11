using Game.Data;
using UnityEngine;

public class ClearProgress : MonoBehaviour
{
    public void Clear()
    {
        PlayerPrefsSafe.DeleteKey(ConstSystem.MISSION_KEY);
        PlayerPrefsSafe.DeleteKey("Prison");
        PlayerPrefsSafe.DeleteKey("TipsPrison");
        PlayerPrefsSafe.DeleteKey("Tips");
        PlayerPrefsSafe.DeleteKey("Papich");
        JSON_saveSystem.DeleteSave();
    }
}
