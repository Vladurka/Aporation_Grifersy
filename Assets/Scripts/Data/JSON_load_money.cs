using UnityEngine;

namespace Game.Data
{
    public class JSON_load_money : MonoBehaviour
    {
        private CoinSystem _coinSystem;

        public void Init()
        {
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
        }

        public void Load()
        {
            if (JSON_saveSystem.SaveExists())
            {
                JSON_playerData data = JSON_saveSystem.Load<JSON_playerData>();
                _coinSystem.Money = data.MoneyData;
                Debug.Log(_coinSystem.Money);
            }
        }
    }
}
