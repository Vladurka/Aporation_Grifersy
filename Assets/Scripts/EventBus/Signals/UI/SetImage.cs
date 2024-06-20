namespace Game.SeniorEventBus.Signals
{
    public class SetImage
    {
        public readonly int Index;
        public readonly bool Active;

        public SetImage(int index, bool active)
        {
            Index = index;
            Active = active;
        }
    }
}