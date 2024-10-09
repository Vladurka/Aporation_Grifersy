using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class ExitTransport
    {
        public readonly Transform Position;

        public ExitTransport() {}

        public ExitTransport(Transform position)
        {
            Position = position;
        }
    }
}
