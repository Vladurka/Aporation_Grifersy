using Game.Player;
using Game.Weapon;
using UnityEngine;
namespace Game.Data
{
    public class LoadDataMission : MonoBehaviour
    {
        private PlayerHealth _playerHP;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private GrenadeThrower _grenadeThrower;
        private VolumeController _volume;

        public void Init()
        {
            _playerHP = ServiceLocator.Current.Get<PlayerHealth>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _volume = ServiceLocator.Current.Get<VolumeController>();
        }

        public void LoadInfo()
        {
            PlayerDataMission data = SaveSystemMission.LoadPlayerData();

            _playerHP.Health = data.HpData;

            _weaponAk.Bullets = data.AKBulletsData;
            _weaponAk.TotalBullets = data.AKTotalBulletsData;

            _rpg.TotalBullets = data.RPGTotalBulletsData;

            _grenadeThrower.Grenades = data.GrenadesData;

            _scopeLevels.ScopeLevel = data.ScopeLevelData;

            _coinSystem.Money = data.MoneyData;

            _volume._volumeSlider.value = data.VolumeData;
        }
    }
}
