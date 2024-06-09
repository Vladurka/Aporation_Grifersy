public interface IObstacleHealth
{
    public float Health { get; set; }
    public void GetDamage(float damage);
    public void Die();
}
