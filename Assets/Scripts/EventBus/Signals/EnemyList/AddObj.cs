using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class AddObj
    {
        public readonly GameObject Enemy;
        public AddObj(GameObject enemy) 
        { 
            Enemy = enemy;
        }
    }
}
