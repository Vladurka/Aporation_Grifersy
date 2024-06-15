namespace Game.SeniorEventBus.Signals
{
    public class UpdateHealth
    {
        public readonly float Amount;

        public UpdateHealth(float amount)
        {
            Amount = amount;
        }
    }
}