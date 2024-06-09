public class HelicopterFixedState : IStates
{
    private Raycasts _raycasts;

    public void Enter()
    {
        _raycasts = ServiceLocator.Current.Get<Raycasts>();
        _raycasts.CanEnterHelicopter = true;
        _raycasts.CanFixHelicopter = false;
    }

    public void Exit()
    {
        _raycasts = ServiceLocator.Current.Get<Raycasts>();
        _raycasts.CanEnterHelicopter = false;
        _raycasts.CanFixHelicopter = true;
    }

    public void UpdateState()
    {
        
    }
}
