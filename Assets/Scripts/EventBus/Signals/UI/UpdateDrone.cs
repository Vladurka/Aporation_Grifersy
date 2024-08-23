namespace Game.SeniorEventBus.Signals
{
    public class UpdateDrone
    {
        public readonly int Amount;

        public UpdateDrone(int amount)
        {
            Amount = amount;
        }
    }
}
