using UnityEngine;

namespace Game.Player
{
    public class PlayerHealth : MonoBehaviour, IPlayerHealth, IService
    {
        public float Health { get; set; } = 10f;

        public void Die()
        {
            Destroy(gameObject);
        }

        public void GetDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
                Die();

            Debug.Log(Health);
        }
    }
}
