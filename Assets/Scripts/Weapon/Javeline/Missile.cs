using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _speed = 500f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _lifeTime = 10f;
    

    [Header("Effects")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private ParticleSystem _flyEffect;

    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private string _tag1 = "Player";
    [SerializeField] private string _tag2 = "AirDefence";

    private GameObject _obj1;
    private GameObject _obj2;

    [SerializeField] private AudioClip _destroySound;

    private AudioSource _audioSource;
    private AudioSource _audioSourceBase;


    [HideInInspector] public Transform Target;


    private void Start()
    {
        _obj1 = GameObject.FindGameObjectWithTag(_tag1);
        _obj2 = GameObject.FindGameObjectWithTag(_tag2);
        _audioSource = _obj1.GetComponent<AudioSource>();
        _audioSourceBase = _obj2.GetComponent<AudioSource>();
        _audioSourceBase.PlayOneShot(_bulletSound);
        StartCoroutine(Effect());
        Invoke("BulletDestroy", _lifeTime);
    }

    void Update()
    {
        if (Target != null)
        {
            transform.LookAt(Target);
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
        _audioSource.PlayOneShot(_destroySound);
        StopAllCoroutines();
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        BulletDestroy();
    }
}
