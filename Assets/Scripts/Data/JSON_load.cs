using Game.Player;
using Game.Weapon;
using UnityEngine;
namespace Game.Data
{
    public class JSON_load : MonoBehaviour
    {
        private PlayerHealth _playerHealth;
        private Movement _playerMove;
        private Helicopter _helicopter;
        private HelicopterStatesController _helicopterStatesController;
        private Car _car;
        private WeaponAk _weaponAk;
        private RPG _rpg;
        private ScopeLevels _scopeLevels;
        private CoinSystem _coinSystem;
        private BaseStates _baseStates;
        private GrenadeThrower _grenadeThrower;
        private ChangeWeapon _changeWeapon;
        private Shop _shop;
        private DroneLouncher _droneLouncher;
        public void Init()
        {
            _playerHealth = ServiceLocator.Current.Get<PlayerHealth>();
            _playerMove = ServiceLocator.Current.Get<Movement>();
            _helicopter = ServiceLocator.Current.Get<Helicopter>();
            _helicopterStatesController = ServiceLocator.Current.Get<HelicopterStatesController>();
            _car = ServiceLocator.Current.Get<Car>();
            _weaponAk = ServiceLocator.Current.Get<WeaponAk>();
            _rpg = ServiceLocator.Current.Get<RPG>();
            _scopeLevels = ServiceLocator.Current.Get<ScopeLevels>();
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
            _baseStates = ServiceLocator.Current.Get<BaseStates>();
            _grenadeThrower = ServiceLocator.Current.Get<GrenadeThrower>();
            _changeWeapon = ServiceLocator.Current.Get<ChangeWeapon>();
            _shop = ServiceLocator.Current.Get<Shop>();
            _droneLouncher = ServiceLocator.Current.Get<DroneLouncher>();
            ConstSystem.CanSave = true;
        }

        public void Load()
        {
            JSON_playerData data = JSON_saveSystem.Load<JSON_playerData>();

            if (data != null)
            {
                if (data.PlayerPositionData != null && data.PlayerPositionData.Length == 3)
                {
                    Vector3 _playerPosition;
                    _playerPosition.x = data.PlayerPositionData[0];
                    _playerPosition.y = data.PlayerPositionData[1];
                    _playerPosition.z = data.PlayerPositionData[2];
                    _playerMove.transform.position = _playerPosition;
                }

                _playerHealth.Health = data.HpData;

                if (data.HelicopterPositionData != null && data.HelicopterConditionData != null && data.HelicopterPositionData.Length == 3)
                {
                    Vector3 _helicopterPosition;
                    _helicopterPosition.x = data.HelicopterPositionData[0];
                    _helicopterPosition.y = data.HelicopterPositionData[1];
                    _helicopterPosition.z = data.HelicopterPositionData[2];
                    _helicopter.transform.position = _helicopterPosition;
                    _helicopterStatesController.HelicopterState = data.HelicopterConditionData;
                }

                if (data.CarPositionData != null && data.CarPositionData.Length == 3)
                {
                    Vector3 _carPosition;
                    _carPosition.x = data.CarPositionData[0];
                    _carPosition.y = data.CarPositionData[1];
                    _carPosition.z = data.CarPositionData[2];
                    _car.transform.position = _carPosition;
                }

                _changeWeapon.SyrgineAmount = data.SyrgineAmount;

                _weaponAk.Bullets = data.AKBulletsData;
                _weaponAk.TotalBullets = data.AKTotalBulletsData;

                _rpg.TotalBullets = data.RPGTotalBulletsData;

                _grenadeThrower.Grenades = data.GrenadesData;

                _scopeLevels.ScopeLevel = data.ScopeLevelData;

                _droneLouncher.DronesAmount = data.DronesAmountData;

                _coinSystem.Money = data.MoneyData;

                _baseStates.BaseLevel = data.BaseLevelData;
                _shop.BaseUpgradeAmount = data.BaseLevelData;
            }

            else
                return;
        }
    }
}

