public class HelicopterDestroyedState : IStates
{
    public void Enter()
    {
        ConstSystem.CanEnterHelicopter = false;
        ConstSystem.CanFixHelicopter = true;
    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
       
    }
}
