namespace Game.SeniorEventBus.Signals
{
    public class BuyMine
    {
        public readonly int Amount;
        public BuyMine(int amount)
        {
            Amount = amount;
        }
    }
}
