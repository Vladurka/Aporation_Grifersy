using UnityEngine;
public class HelicopterFixedState : IStates
{
    public void Enter()
    {
        if (MissionsController.MissionCondition >= 4)
        {
            ConstSystem.CanEnterHelicopter = true;
            ConstSystem.CanFixHelicopter = false;
        }
    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
        
    }

    private void CanFixHelicopter()
    {

    }
}
