public interface IEnemyHealth
{
    public bool IsDead { get; set; }
    public int KillCost { get; set; }
    public float Health { get; set; }
    public void GetDamage(float damage);
    public void Die();
}
