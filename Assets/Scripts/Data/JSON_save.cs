using Game.Player;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using Game.Weapon;
using UnityEngine;
namespace Game.Data
{
    public class JSON_save : MonoBehaviour
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
        private BaseDroneLouncher _droneLouncher;

        private EventBus _eventBus;
        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<SaveDataSignal>(Save);

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
            _droneLouncher = ServiceLocator.Current.Get<BaseDroneLouncher>();
            ConstSystem.CanSave = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SaveInfo();
        }

        private void Save(SaveDataSignal save)
        {
            SaveInfo();
        }

        public void SaveInfo()
        {
            if (ConstSystem.CanSave)
            {
                JSON_playerData data = new JSON_playerData
                {
                    HelicopterConditionData = _helicopterStatesController.HelicopterState,

                    HelicopterPositionData = new float[3]
                    {
                    _helicopter.transform.position.x,
                    _helicopter.transform.position.y,
                    _helicopter.transform.position.z,
                    },

                    CarPositionData = new float[3]
                    {
                    _car.transform.position.x,
                    _car.transform.position.y,
                    _car.transform.position.z,
                    },

                    HpData = _playerHealth.Health,

                    PlayerPositionData = new float[3]
                    {
                    _playerMove.transform.position.x,
                    _playerMove.transform.position.y,
                    _playerMove.transform.position.z
                    },

                    SyrgineAmount = _changeWeapon.SyrgineAmount,

                    AKBulletsData = _weaponAk.Bullets,
                    AKTotalBulletsData = _weaponAk.TotalBullets,

                    RPGTotalBulletsData = _rpg.TotalBullets,
                    GrenadesData = _grenadeThrower.Grenades,

                    ScopeLevelData = _scopeLevels.ScopeLevel,

                    DronesAmountData = _droneLouncher.DronesAmount,

                    MoneyData = _coinSystem.Money,

                    BaseLevelData = _baseStates.BaseLevel,
                };
                JSON_saveSystem.Save(data);
            }
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<SaveDataSignal>(Save);
        }
    }
}
