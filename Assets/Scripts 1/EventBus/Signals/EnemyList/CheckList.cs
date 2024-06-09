using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class CheckList
    {
        public readonly Vector3 Position;
        public readonly float Range;
        public CheckList(Vector3 position, float range)
        {
            Position = position;
            Range = range;
        }
    }
}



    
