using Game.Player;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    [SerializeField] private float _radius = 3;
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        foreach (Collider hit in hits)
        {
            if (hit.transform.TryGetComponent(out IPlayerHealth player))
                player.GetDamage(100f);
        }
        Destroy(gameObject);
    }
}
