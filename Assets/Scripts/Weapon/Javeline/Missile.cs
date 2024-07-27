using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _lifeTime = 10f;
    [SerializeField] private float _rotationSpeed = 85f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _flyEffect;

    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private string _tag = "Player";

    private GameObject _mainCharacter;

    private AudioSource _audioSource;


    [HideInInspector] public Transform Target;


    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag(_tag);
        _audioSource = _mainCharacter.GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_bulletSound);
        StartCoroutine(Effect());
        Invoke("BulletDestroy", _lifeTime);
    }

    void Update()
    {
        if (Target != null)
        {
            Vector3 dir = Target.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            float distanceThisFrame = _speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                BulletDestroy();
            }
            else
            {
                transform.Translate(Vector3.forward * distanceThisFrame);
            }
        }
    }

    private IEnumerator Effect()
    {
        Instantiate(_flyEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Effect());
    }

    private void BulletDestroy()
    {
        _audioSource.Play();
        StopAllCoroutines();
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        BulletDestroy();
    }
}
