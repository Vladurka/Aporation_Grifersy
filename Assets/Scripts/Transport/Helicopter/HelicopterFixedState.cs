public class HelicopterFixedState : IStates
{
    private Raycasts _raycasts;
    private CoinSystem _coinSystem;

    private int _fixPrice = 1500;

    public void Enter()
    {
        _coinSystem = ServiceLocator.Current.Get<CoinSystem>();
        //if (_coinSystem.Money >= _fixPrice)
        //{
            _raycasts = ServiceLocator.Current.Get<Raycasts>();
            _raycasts.CanEnterHelicopter = true;
            _raycasts.CanFixHelicopter = false;
            //_coinSystem.SpendMoney(_fixPrice);
        //}
    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
        
    }
}
