using Game.Player;
using Game.Weapon;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game.Data
{
    public static class SaveSystemMission
    {
        public static void SavePlayerData(PlayerHealth playerHP, VolumeController volume, GrenadeThrower grenade, ScopeLevels scopeLevels, WeaponAk ak, RPG rpg, CoinSystem coins)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + ConstSystem.DATA_PATH;
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerDataMission data = new PlayerDataMission(playerHP, volume, grenade, scopeLevels, ak, rpg, coins);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerDataMission LoadPlayerData()
        {
            string path = Application.persistentDataPath + ConstSystem.DATA_PATH;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerDataMission data = formatter.Deserialize(stream) as PlayerDataMission;

                return data;
            }
            else
            {
                Debug.Log("The file wasn't find in" + path);
                return null;
            }
        }
    }
}
