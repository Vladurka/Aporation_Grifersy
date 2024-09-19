using UnityEngine;
using Game.Player;
using System.Collections;

namespace Game.Enemy
{
    public class EnemyExpolsion : MonoBehaviour
    {
        [SerializeField] private float _damage = 40f;
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private ParticleSystem _explosionEffect;

        private GameObject _mainCharacter;

        private bool _isExploded = false;

        private void Start()
        {   
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");

            if (!_mainCharacter.Equals(null))
                StartCoroutine(FindPlayer());
        }

        private void Update()
        {
            if (!_mainCharacter.Equals(null))
            {
                if (Vector3.Distance(transform.position, _mainCharacter.transform.position) <= _explosionRadius && _mainCharacter.activeSelf)
                    Explode();
            }
        }

        private void Explode()
        {
            if (!_isExploded)
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

        private IEnumerator FindPlayer()
        {
            if (_mainCharacter == null)
            {
                _mainCharacter = GameObject.FindGameObjectWithTag("Player");
                Debug.Log(_mainCharacter);
                yield return new WaitForSeconds(3f);
                StartCoroutine(FindPlayer());
            }

            if (_mainCharacter != null)
                yield break;
        }

    }
}
