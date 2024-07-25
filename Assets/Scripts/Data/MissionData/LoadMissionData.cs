using UnityEngine;
using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    public class LoadMissionData : MonoBehaviour, IService
    {
        private PlayerHealth _playerHealth;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private GrenadeThrower _grenadeThrower;

        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>(); ;
        }

        public void LoadInfo()
        {
            PlayerData data = SaveSystem.LoadPlayerData();

            _playerHealth.Health = data.HpData;

            _weaponAk.Bullets = data.AKBulletsData;
            _weaponAk.TotalBullets = data.AKTotalBulletsData;

            _rpg.TotalBullets = data.RPGTotalBulletsData;

            _grenadeThrower.Grenades = data.GrenadesData;

            _scopeLevels.ScopeLevel = data.ScopeLevelData;

            _coinSystem.Money = data.MoneyData;
        }
    }
}