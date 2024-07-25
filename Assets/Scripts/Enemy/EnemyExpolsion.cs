using UnityEngine;
using Game.Player;
using Unity.VisualScripting;

namespace Game.Enemy
{
    public class EnemyExpolsion : MonoBehaviour
    {
        [SerializeField] private float _damage = 40f;
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private GameObject _mainCharacter;

        private bool _isExploded = false;

        private void Start()
        {   
            if(_mainCharacter == null)
                _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (_mainCharacter != null)
            {
                if (Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _explosionRadius && _mainCharacter.activeSelf)
                {
                    Explode();
                }
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
