namespace Game.SeniorEventBus.Signals
{
    public class GetScope
    {
        public readonly int Level;
        public GetScope(int level)
        {
            Level = level;
        }
    }
}