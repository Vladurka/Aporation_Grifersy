namespace Game.SeniorEventBus.Signals
{
    public class SetSpeedometer
    {
        public readonly bool State;
        public SetSpeedometer(bool state)
        {
            State = state;
        }
    }
}
