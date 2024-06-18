using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class InstantRocket
    {
        public readonly Vector3 Dir;
        public readonly float Force;

        public InstantRocket(Vector3 dir, float force)
        {
            Dir = dir;
            Force = force;
        }
    }
}
