using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class SetAimCamera
    {
        public readonly Camera AimCamera;

        public SetAimCamera(Camera aimCamera)
        {
            AimCamera = aimCamera;
        }
    }
}

