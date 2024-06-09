public class CarDestroyedState : IStates
{
    private Raycasts _raycasts;

    public void Enter()
    {
        _raycasts = ServiceLocator.Current.Get<Raycasts>();
        _raycasts.CanEnterCar = false;
        _raycasts.CanFixCar = true;
    }

    public void Exit()
    {
        _raycasts = ServiceLocator.Current.Get<Raycasts>();
        _raycasts.CanEnterCar = true;
        _raycasts.CanFixCar = false;
    }

    public void UpdateState()
    {
        
    }
}
