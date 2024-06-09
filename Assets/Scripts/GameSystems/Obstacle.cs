using System;
using UnityEngine;
public class Obstacle : MonoBehaviour, IObstacleHealth
{
    public float Health { get; set; } = 1;
    public void Die()
    {
        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
            Die();
    }
}



