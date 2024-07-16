using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _flyEffect;

    [SerializeField] private AudioClip _bulletSound;

    private GameObject _mainCharacter;

    private AudioSource _audioSource;


    public Transform Target;


    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
        _audioSource = _mainCharacter.GetComponent<AudioSource>();
        _audioSource.PlayOneShot(_bulletSound);
        StartCoroutine(Effect());
        Invoke("SetFalse", 7f);
    }

    void Update()
    {
        if (Target != null)
        {
            transform.LookAt(Target.position);
            Vector3 dir = Target.position - transform.position;
            float distanceThisFrame = _speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                BulletDestroy();
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }

    private IEnumerator Effect()
    {
        Instantiate(_flyEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.05f);
        Invoke("SetFalse", 7f);
        StartCoroutine(Effect());
    }

    private void BulletDestroy()
    {
        _audioSource.Play();
        StopAllCoroutines();
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void SetFalse()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        BulletDestroy();
    }
}
