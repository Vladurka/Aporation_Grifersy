namespace Game.SeniorEventBus.Signals
{
    public class BuyAkBullets
    {
        public readonly int Amount;
        public BuyAkBullets(int amount) 
        {
            Amount = amount;
        }
    }
}