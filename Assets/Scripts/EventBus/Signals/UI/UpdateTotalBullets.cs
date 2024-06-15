namespace Game.SeniorEventBus.Signals
{
    public class UpdateTotalBullets
    {
        public readonly int Amount;

        public UpdateTotalBullets(int amount)
        {
            Amount = amount;
        }
    }
}