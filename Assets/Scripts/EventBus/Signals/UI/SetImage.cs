namespace Game.SeniorEventBus.Signals
{
    public class SetImage
    {
        public readonly int Index;

        public SetImage(int index)
        {
            Index = index;
        }
    }
}