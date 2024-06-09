namespace Game.SeniorEventBus.Signals
{
    public class BuyRpgBullets
    {
        public readonly int Amount;
        public BuyRpgBullets(int amount)
        {
            Amount = amount;
        }
    }
}
