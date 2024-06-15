namespace Game.SeniorEventBus.Signals
{
    public class UpdateCurrentBullets
    {
        public readonly int Amount;

        public UpdateCurrentBullets(int amount)
        {
            Amount = amount;
        }
    }
}
