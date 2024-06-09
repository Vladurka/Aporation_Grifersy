namespace Game.SeniorEventBus
{
    public class CallbackWithPriority
    {
        public readonly int Priority;
        public readonly object Callback;

        public CallbackWithPriority(int priority, object callback)
        {
            Priority = priority;
            Callback = callback;
        }
    }
}