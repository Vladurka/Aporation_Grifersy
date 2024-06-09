using UnityEngine;
using Game.Player;

namespace Game.Enemy
{
    public class EnemyExpolsion : MonoBehaviour
    {
        [SerializeField] private float _damage = 4f;
        [SerializeField] private float _explosionRadius = 2f;

        [SerializeField] private GameObject _mainCharacter;

        [SerializeField] private ParticleSystem _explosionEffect;

        private bool _isExploded = false;

        private void Update()
        {
            if(Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _explosionRadius && _mainCharacter.activeSelf)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (_isExploded == false)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

                foreach (Collider hit in hits)
                {
                    if (hit.transform.TryGetComponent(out IPlayerHealth player))
                        player.GetDamage(_damage);
                }
                Instantiate(_explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);

                _isExploded = true;
            }
        }
    }
}
