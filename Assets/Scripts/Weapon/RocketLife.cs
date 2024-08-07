using Game.Player;
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
        [SerializeField] private string _tag = "Player";

        private GameObject _mainCharacter;

        private AudioSource _audioSource;

        
        private void Start()
        {
            _mainCharacter = GameObject.FindGameObjectWithTag(_tag);
            _audioSource = _mainCharacter.GetComponent<AudioSource>();
            StartCoroutine(Effect());
            Invoke("SetFalse", 7f);
        }

        private IEnumerator Effect()
        {
            Instantiate(_flyEffect, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.05f);

            StartCoroutine(Effect());
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

                if (hit.transform.TryGetComponent(out ITankHealth tank))
                    tank.Destroy();
            }


            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
            foreach (Collider hit in colliders)
            {
                if (hit.transform.TryGetComponent(out IPlayerHealth player))
                    player.GetDamage(20f);
            }
            BulletDestroy();
        }

        private void BulletDestroy()
        {
            _audioSource.PlayOneShot(_explosion);
            StopAllCoroutines();
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

        private void SetFalse()
        {
            gameObject.SetActive(false);
        }
    }
}
