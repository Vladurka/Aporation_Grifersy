namespace Game.SeniorEventBus.Signals
{
    public class BuyGrenades
    {
        public readonly int Amount;
        public BuyGrenades(int amount)
        {
            Amount = amount;
        }
    }
}
