namespace Game.Enemy
{
    public interface IEnemyMove
    {
        public bool IsDetected { get; set; }

        public void Walk();
        public void EnemyDetected();
    }
}
