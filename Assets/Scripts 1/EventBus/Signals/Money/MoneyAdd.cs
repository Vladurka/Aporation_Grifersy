namespace Game.SeniorEventBus.Signals
{
    public class MoneyAdd 
    {
        public readonly int Amount;

        public MoneyAdd(int amount)
        {
            Amount = amount;
        }
    }
}
