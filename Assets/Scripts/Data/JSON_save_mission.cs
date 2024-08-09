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
        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            ConstSystem.CanSave = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SaveInfo();
        }

        private void SaveInfo()
        {
            if (ConstSystem.CanSave)
            {
                JSON_playerData data = new JSON_playerData
                {
                    HpData = _playerHealth.Health,

                    AKBulletsData = _weaponAk.Bullets,
                    AKTotalBulletsData = _weaponAk.TotalBullets,

                    RPGTotalBulletsData = _rpg.TotalBullets,
                    GrenadesData = _grenadeThrower.Grenades,

                    ScopeLevelData = _scopeLevels.ScopeLevel,

                    MoneyData = _coinSystem.Money,
                };

                JSON_saveSystem.Save(data);
            }
        }
    }
}

