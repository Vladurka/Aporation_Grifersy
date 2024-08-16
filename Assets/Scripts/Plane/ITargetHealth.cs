public interface ITargetHealth 
{
    public float Health { get; set; }
    public void GetDamage(float damage);
    public void Destroy();
}
