namespace Game.Player
{
    public interface IPlayerHealth
    {
        public float Health { get; set; }
        
        public void GetDamage(float damage);
        public void Die();
    }
}
