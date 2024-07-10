using Game.Player;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayerHealth player))
            player.GetDamage(100f);

        if (other.TryGetComponent(out IHelicopterHealth helicopter) && !_mainCharacter.activeSelf)
            helicopter.GetDamage(100f);

        if (other.TryGetComponent(out ICarHealth car) && !_mainCharacter.activeSelf)
            car.GetDamage(100f);
    }
}
