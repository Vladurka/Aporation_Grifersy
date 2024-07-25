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
        public float[] PlayerPositionData;
        public int AKBulletsData;
        public int AKTotalBulletsData;
        public int RPGTotalBulletsData;
        public int GrenadesData;
        public int MoneyData;
        public float HpData;
        public int ScopeLevelData;
        public int MissionsData;
        public int BaseLevelData;

        public PlayerData(PlayerHealth playerHP, Movement playerMove, GrenadeThrower grenade, ScopeLevels scopeLevels, Helicopter helicopter, HelicopterStatesController helicopterStatesController, Car car, WeaponAk ak, RPG rpg, CoinSystem coins, BaseStates baseStates)
        {
            HelicopterConditionData = helicopterStatesController.HelicopterState;

            HelicopterPositionData = new float[3];
            HelicopterPositionData[0] = helicopter.transform.position.x;
            HelicopterPositionData[1] = helicopter.transform.position.y;
            HelicopterPositionData[2] = helicopter.transform.position.z;

            CarPositionData = new float[3];
            CarPositionData[0] = car.transform.position.x;
            CarPositionData[1] = car.transform.position.y;
            CarPositionData[2] = car.transform.position.z;

            HpData = playerHP.Health;

            PlayerPositionData = new float[3];
            PlayerPositionData[0] = playerMove.transform.position.x;
            PlayerPositionData[1] = playerMove.transform.position.y;
            PlayerPositionData[2] = playerMove.transform.position.z;

            AKBulletsData = ak.Bullets;
            AKTotalBulletsData = ak.TotalBullets;

            RPGTotalBulletsData = rpg.TotalBullets;

            GrenadesData = grenade.Grenades;

            ScopeLevelData = scopeLevels.ScopeLevel;

            MoneyData = coins.Money;

            BaseLevelData = baseStates.BaseLevel;
        }

        public PlayerData(PlayerHealth playerHP, GrenadeThrower grenade, ScopeLevels scopeLevels, WeaponAk ak, RPG rpg, CoinSystem coins)
        {

            HpData = playerHP.Health;

            AKBulletsData = ak.Bullets;
            AKTotalBulletsData = ak.TotalBullets;

            RPGTotalBulletsData = rpg.TotalBullets;

            GrenadesData = grenade.Grenades;

            ScopeLevelData = scopeLevels.ScopeLevel;

            MoneyData = coins.Money;
        }
    }
}
