namespace Game.Enemy
{
    public interface IEnemyMove
    {
        public bool IsDetected { get; set; }
        public bool IsDead { get; set; }

        public void Walk();
        public void EnemyDetected();
    }
}
