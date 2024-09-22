using Game.Data;
using UnityEngine;

public class ClearProgress : MonoBehaviour
{
    public void Clear()
    {
        PlayerPrefsSafe.DeleteKey(ConstSystem.MISSION_KEY);
        PlayerPrefsSafe.DeleteKey(ConstSystem.PRISON_ENDED);
        PlayerPrefsSafe.DeleteKey(ConstSystem.PRISON_TIPS);
        PlayerPrefsSafe.DeleteKey(ConstSystem.GAME_TIPS);
        PlayerPrefsSafe.DeleteKey(ConstSystem.PAPICH);
        JSON_saveSystem.DeleteSave();
    }
}
