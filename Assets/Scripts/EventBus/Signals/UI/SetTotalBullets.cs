namespace Game.SeniorEventBus.Signals
{
    public class SetTotalBullets
    {
        public readonly bool State;

        public SetTotalBullets(bool state)
        {
            State = state;
        }
    }
}