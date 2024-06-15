namespace Game.SeniorEventBus.Signals
{
    public class SetCurrentBullets
    {
        public readonly bool State;

        public SetCurrentBullets(bool state)
        {
            State = state;
        }
    }
}