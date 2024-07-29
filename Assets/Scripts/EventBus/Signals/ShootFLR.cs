using UnityEngine;
namespace Game.SeniorEventBus.Signals
{
    public class ShootFLR
    {
        public readonly Transform Target;

        public ShootFLR(Transform target)
        {
            Target = target;
        }
    }
}
