namespace Game.SeniorEventBus.Signals
{
    public class ShakeCamera
    {
        public readonly float Force;
        public readonly float Duration;
        public ShakeCamera(float force, float duration)
        {
            Force = force;
            Duration = duration;
        }
    }
}
