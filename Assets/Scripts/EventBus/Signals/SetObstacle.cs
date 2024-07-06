using UnityEngine;

namespace Game.SeniorEventBus.Signals
{
    public class SetObstacle
    {
        public readonly GameObject Obstacle;
        public SetObstacle(GameObject obstacle)
        {
            Obstacle = obstacle;
        }
    }
}
