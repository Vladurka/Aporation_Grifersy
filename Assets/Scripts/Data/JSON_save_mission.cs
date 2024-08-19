using Game.Player;
using Game.Weapon;
using UnityEngine;
namespace Game.Data
{
    public class JSON_save_mission : MonoBehaviour
    {
        private PlayerHealth _playerHealth;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private GrenadeThrower _grenadeThrower;
        private ChangeWeapon _changeWeapon;
        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _changeWeapon = ServiceLocator.Current.Get<ChangeWeapon>();
            ConstSystem.CanSave = true;
        }

        public void SaveInfo()
        {
            JSON_playerData existingData = JSON_saveSystem.Load<JSON_playerData>();

            if (existingData == null)
            {
                existingData = new JSON_playerData();
            }

            existingData.HpData = _playerHealth.Health;
            existingData.SyrgineAmount = _changeWeapon.SyrgineAmount;
            existingData.AKBulletsData = _weaponAk.Bullets;
            existingData.AKTotalBulletsData = _weaponAk.TotalBullets;
            existingData.RPGTotalBulletsData = _rpg.TotalBullets;
            existingData.GrenadesData = _grenadeThrower.Grenades;
            existingData.ScopeLevelData = _scopeLevels.ScopeLevel;
            existingData.MoneyData = _coinSystem.Money;

            JSON_saveSystem.Save(existingData);
        }
    }
}

