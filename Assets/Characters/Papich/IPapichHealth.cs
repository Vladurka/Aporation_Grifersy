public interface IPapichHealth 
{
    public float Health { get; set; }
    public void GetDamage(float damage);
    public void Die();

    public void Say();
}
