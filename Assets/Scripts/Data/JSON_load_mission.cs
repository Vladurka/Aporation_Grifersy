using Game.Player;
using Game.Weapon;
using UnityEngine;
namespace Game.Data
{
    public class JSON_load_mission : MonoBehaviour
    {
        [SerializeField] private bool _loadDrones = true;

        private PlayerHealth _playerHealth;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private GrenadeThrower _grenadeThrower;
        private ChangeWeapon _changeWeapon;
        private BaseDroneLouncher _droneLouncher;
        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _changeWeapon = ServiceLocator.Current.Get<ChangeWeapon>();

            if (_loadDrones)
                _droneLouncher = ServiceLocator.Current.Get<BaseDroneLouncher>();
        }

        public void Load()
        {
            if (JSON_saveSystem.SaveExists())
            {
                JSON_playerData data = JSON_saveSystem.Load<JSON_playerData>();

                _playerHealth.Health = data.HpData;

                _changeWeapon.SyrgineAmount = data.SyrgineAmount;

                _weaponAk.Bullets = data.AKBulletsData;
                _weaponAk.TotalBullets = data.AKTotalBulletsData;

                _rpg.TotalBullets = data.RPGTotalBulletsData;

                _grenadeThrower.Grenades = data.GrenadesData;

                _scopeLevels.ScopeLevel = data.ScopeLevelData;

                if(_loadDrones)
                    _droneLouncher.DronesAmount = data.DronesAmountData;

                _coinSystem.Money = data.MoneyData;
            }
        }
    }
}

