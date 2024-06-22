using Game.Player;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayerHealth player))
            player.GetDamage(100f);
    }
}
