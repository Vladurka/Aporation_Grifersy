using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    public static class SaveSystem
    {
        public static void SavePlayerData(PlayerHealth player, Movement playerMove, VolumeController volume, SetMine setMine, GrenadeThrower grenadeThrower,ScopeLevels scopeLevels, Helicopter helicopter, HelicopterStatesController helicopterStatesController, Car car, WeaponAk ak, RPG rpg, CoinSystem coins, BaseStates baseStates)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + ConstSystem.DATA_PATH;
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player, playerMove, volume, setMine, grenadeThrower, scopeLevels, helicopter, helicopterStatesController, car, ak, rpg, coins, baseStates);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData LoadPlayerData()
        {
            string path = Application.persistentDataPath + ConstSystem.DATA_PATH;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;

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
