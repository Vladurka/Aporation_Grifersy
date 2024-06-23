public interface ICarHealth 
{
    public float Health { get; set; }

    public void GetDamage(float damage);
    public void Die();
}
