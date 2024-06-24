public class HelicopterDestroyedState : IStates
{
    private Raycasts _raycasts;
    public void Enter()
    {
        _raycasts = ServiceLocator.Current.Get<Raycasts>();
        _raycasts.CanEnterHelicopter = false;
        _raycasts.CanFixHelicopter = true;
    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
       
    }
}
