using Game.Player;
using Game.Weapon;
using UnityEngine;

namespace Game.Data
{
    public class SaveDataMission : MonoBehaviour
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SaveInfo();
        }

        public void SaveInfo()
        {
            SaveSystemMission.SavePlayerData(_playerHP, _volume, _grenadeThrower, _scopeLevels, _weaponAk, _rpg, _coinSystem);
        }
    }
}
