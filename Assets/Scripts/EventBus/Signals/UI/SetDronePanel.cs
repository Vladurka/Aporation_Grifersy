namespace Game.SeniorEventBus.Signals
{
    public class SetDronePanel
    {
        public readonly bool State;

        public SetDronePanel(bool state)
        {
            State = state;
        }
    }
}
