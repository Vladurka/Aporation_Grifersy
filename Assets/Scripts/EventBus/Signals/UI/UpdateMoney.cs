namespace Game.SeniorEventBus.Signals
{
    public class UpdateMoney
    {
        public readonly int Amount;

        public UpdateMoney(int amount)
        {
            Amount = amount;
        }
    }
}