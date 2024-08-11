using Game.Data;
using UnityEngine;

public class ClearProgress : MonoBehaviour
{
    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        JSON_saveSystem.DeleteSave();
    }
}
