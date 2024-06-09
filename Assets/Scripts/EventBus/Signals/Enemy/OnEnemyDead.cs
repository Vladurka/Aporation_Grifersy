namespace Game.SeniorEventBus.Signals
{
    public class OnEnemyDead
    {
        public readonly int Value;

        public OnEnemyDead(int value)
        {
            Value = value;
        }
    }
}
