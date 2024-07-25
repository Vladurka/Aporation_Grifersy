public class HelicopterDestroyedState : IStates
{
    public void Enter()
    {
        ConstSystem.CanEnterHelicopter = false;
    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
       
    }
}
