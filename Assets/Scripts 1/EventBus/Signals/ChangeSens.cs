namespace Game.SeniorEventBus.Signals
{
    public class ChangeSens 
    {
        public readonly float Value;

        public ChangeSens(float value)
        {
            Value = value;
        }
    }
}
