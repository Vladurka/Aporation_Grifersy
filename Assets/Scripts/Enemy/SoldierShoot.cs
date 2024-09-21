using Game.Player;
using System.Collections;
using UnityEngine;

public class SoldierShoot : MonoBehaviour
{
    [SerializeField] private float _damage = 30f;
    [SerializeField] private Transform _shootPosition;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2.5f);

        RaycastHit hit;
        if (Physics.Raycast(_shootPosition.position, _shootPosition.forward, out hit))
        {
            _animator.SetTrigger("Shoot");

            if (hit.collider.TryGetComponent(out IPlayerHealth player))
                player.GetDamage(_damage);
        }
        StartCoroutine(Shoot());
    }
}
