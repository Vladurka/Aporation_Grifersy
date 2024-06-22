namespace Game.SeniorEventBus.Signals
{
    public class EnablePause
    {
        public readonly bool State;
        public EnablePause(bool state)
        {
            State = state;
        }
    }
}
