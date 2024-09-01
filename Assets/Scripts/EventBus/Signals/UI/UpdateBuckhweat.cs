namespace Game.SeniorEventBus.Signals
{
    public class UpdateBuckhweat
    {
        public readonly int Amount;
        public UpdateBuckhweat(int amount)
        {
            Amount = amount;
        }
    }
}
