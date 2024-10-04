using Game.Player;
using System.Collections;
using UnityEngine;

namespace Game.Weapon
{
    public class RocketLife : MonoBehaviour
    {
        [Header("Rocket stats")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _explosionRadius = 3f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private ParticleSystem _flyEffect;

        [SerializeField] private AudioClip _explosion;

        private AudioSource _audioSource;
        private MeshRenderer _meshRenderer;

        private bool _isExploded = false;

        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            StartCoroutine(Effect());
            Invoke("BulletDestroy", 7f);
        }

        private IEnumerator Effect()
        {
            Instantiate(_flyEffect, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.05f);

            StartCoroutine(Effect());
        }
        private void Kill()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (Collider hit in hits)
            {
                if (hit.transform.TryGetComponent(out IEnemyHealth enemy))
                    enemy.GetDamage(_damage);

                if (hit.transform.TryGetComponent(out ITargetHealth target) && !target.IsArmored)
                    target.Destroy();

                if (hit.transform.TryGetComponent(out IPlayerHealth player))
                    player.GetDamage(20f);
            }
          
            BulletDestroy();
        }

        private void BulletDestroy()
        {
            if (!_isExploded)
            {
                _isExploded = true;
                _audioSource.pitch = Random.Range(0.8f, 1.2f);
                _audioSource.PlayOneShot(_explosion);
                StopAllCoroutines();
                Instantiate(_explosionEffect, transform.position, Quaternion.identity);
                _meshRenderer.enabled = false;
                Destroy(gameObject, 2f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Kill();
        }

    }
}
