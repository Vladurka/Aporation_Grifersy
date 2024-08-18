namespace Game.SeniorEventBus.Signals
{
    public class UpdateSyrgine
    {
        public readonly int Amount;
        public UpdateSyrgine(int amount)
        {
            Amount = amount;
        }
    }
}
