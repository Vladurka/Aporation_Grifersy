using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    [System.Serializable]
    public class PlayerDataMission
    {
        public int AKBulletsData;
        public int AKTotalBulletsData;
        public int RPGTotalBulletsData;
        public int GrenadesData;
        public int MoneyData;
        public float VolumeData;
        public float HpData;
        public int ScopeLevelData;

        public PlayerDataMission(PlayerHealth playerHP, VolumeController volume, GrenadeThrower grenade, ScopeLevels scopeLevels, WeaponAk ak, RPG rpg, CoinSystem coins)
        {

            HpData = playerHP.Health;

            AKBulletsData = ak.Bullets;

            AKTotalBulletsData = ak.TotalBullets;

            RPGTotalBulletsData = rpg.TotalBullets;

            GrenadesData = grenade.Grenades;

            ScopeLevelData = scopeLevels.ScopeLevel;

            MoneyData = coins.Money;

            VolumeData = volume._volumeSlider.value;
        }
    }
}
