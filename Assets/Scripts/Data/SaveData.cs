using UnityEngine;
using Game.Player;
using Game.Weapon;

namespace Game.Data
{
    public class SaveData : MonoBehaviour, IService
    {
        private PlayerHealth _player;
        private Movement _playerMove;
        private Helicopter _helicopter;
        private HelicopterStatesController _helicopterStatesController;
        private Car _car;
        private CarStatesController _carStatesController;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private BaseStates _baseStates;
        private GrenadeThrower _grenadeThrower;
        private SetMine _setMine;
        private VolumeController _volume;
        public void Init()
        {
            _player = ServiceLocator.Current.Get<PlayerHealth>();
            _playerMove = ServiceLocator.Current.Get<Movement>();
            _helicopter = ServiceLocator.Current.Get<Helicopter>();
            _helicopterStatesController = ServiceLocator.Current.Get<HelicopterStatesController>();
            _car = ServiceLocator.Current.Get<Car>();
            _carStatesController = ServiceLocator.Current.Get<CarStatesController>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _baseStates = ServiceLocator.Current.Get<BaseStates>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _setMine = ServiceLocator.Current.Get<SetMine>();
            _volume = ServiceLocator.Current.Get<VolumeController>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
                SaveInfo();
        }

        public void SaveInfo()
        {
            SaveSystem.SavePlayerData(_player, _playerMove, _volume, _setMine, _grenadeThrower, _scopeLevels, _helicopter, _helicopterStatesController, _car, _carStatesController, _weaponAk, _rpg, _coinSystem, _coinSystem, _baseStates); 
        }
    }
}
