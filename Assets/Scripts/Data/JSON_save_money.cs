using UnityEngine;

namespace Game.Data
{
    public class JSON_save_money : MonoBehaviour
    {
        private CoinSystem _coinSystem;

        public void Init()
        {
            _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
        }

        public void Save()
        {
            JSON_playerData existingData = JSON_saveSystem.Load<JSON_playerData>();

            if (existingData == null)
                existingData = new JSON_playerData();

            existingData.MoneyData = _coinSystem.Money;

            Debug.Log(existingData.MoneyData);

            JSON_saveSystem.Save(existingData);
        }
    }
}
