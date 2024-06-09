using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class RemoveObj
    {
        public readonly GameObject Enemy;
        public RemoveObj(GameObject enemy)
        {
            Enemy = enemy;
        }
    }
}
