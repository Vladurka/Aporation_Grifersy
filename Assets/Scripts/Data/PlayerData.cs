using UnityEngine;
using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public float[] HelicopterPositionData;
        public string HelicopterConditionData;
        public float[] CarPositionData;
        public string CarConditionData;
        public float[] PLayerPositionData;
        public int AKBulletsData;
        public int AKTotalBulletsData;
        public int RPGTotalBulletsData;
        public int GrenadesData;
        public int MinesData;
        public int MoneyData;
        public float HpData;
        public int ScopeLevelData;
        public int MissionsData;
        public int BaseLevelData;

        public PlayerData(PlayerHealth playerHP, Movement playerMove, SetMine setMine, GrenadeThrower grenade, ScopeLevels scopeLevels, Helicopter helicopter, HelicopterStatesController helicopterStatesController, Car car, CarStatesController carStatesController, WeaponAk ak, RPG rpg, CoinSystem coins, BaseStates baseStates)
        {
            HelicopterConditionData = helicopterStatesController.HelicopterState;

            HelicopterPositionData = new float[3];
            HelicopterPositionData[0] = helicopter.transform.position.x;
            HelicopterPositionData[1] = helicopter.transform.position.y;
            HelicopterPositionData[2] = helicopter.transform.position.z;

            CarConditionData = carStatesController.CarState;

            CarPositionData = new float[3];
            CarPositionData[0] = car.transform.position.x;
            CarPositionData[1] = car.transform.position.y;
            CarPositionData[2] = car.transform.position.z;

            HpData = playerHP.Health;

            PLayerPositionData = new float[3];
            PLayerPositionData[0] = playerMove.transform.position.x;
            PLayerPositionData[1] = playerMove.transform.position.y;
            PLayerPositionData[2] = playerMove.transform.position.z;

            AKBulletsData = ak.Bullets;

            AKTotalBulletsData = ak.TotalBullets;

            RPGTotalBulletsData = rpg.TotalBullets;

            MinesData = setMine.TotalBullets;

            GrenadesData = grenade.Grenades;

            ScopeLevelData = scopeLevels.ScopeLevel;

            MoneyData = coins.Money;

            BaseLevelData = baseStates.BaseLevel;
        }
    }
}
