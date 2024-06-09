using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using System.Collections;
using UnityEngine;

namespace Game.Weapon
{
    public class RocketLife : MonoBehaviour
    {
        [Header("Rocket stats")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _explosionRadius = 5f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private ParticleSystem _flyEffect;

        [SerializeField] private AudioClip _explosion;

        private GameObject _mainCharacter;

        private AudioSource _audioSource;


        private void Start()
        {
            _mainCharacter = GameObject.FindGameObjectWithTag("Player");
            _audioSource = _mainCharacter.GetComponent<AudioSource>();
            StartCoroutine(Effect());
        }
        private void Update()
        {
            Destroy(gameObject, 7f);
        }
        private void OnCollisionEnter(Collision collision)
        {
            Kill();
        }
            
        private void Kill()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (Collider hit in hits)
            {
                if (hit.transform.TryGetComponent(out IEnemyHealth enemy))
                    enemy.GetDamage(_damage);
            }
            BulletDestroy();
        }

        private IEnumerator Effect()
        {
            Instantiate(_flyEffect, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.05f);

            StartCoroutine(Effect());
        }

        private void BulletDestroy()
        {
            _audioSource.PlayOneShot(_explosion);
            StopAllCoroutines();
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
