using Game.Enemy;
using UnityEngine;

public class EnemyHelicopter : MonoBehaviour, ITargetHealth
{
    [SerializeField] private float _time = 35f;

    [SerializeField] private bool _isPapich = false;

    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _mainCharacter;

    [SerializeField] private Material _destroyedMaterial;

    private AudioSource _audioSource;
    private Animator _animator;
    private Animator[] _animators;
    private MeshRenderer[] _meshRenderers;
    private Rigidbody _rb;

    public float Health { get ; set; } = 100f;
    public bool IsArmored { get; set; } = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animators = GetComponentsInChildren<Animator>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _animator.SetTrigger("Fly");
        Invoke("Drop", _time);
        _audioSource.Play();
    }
    


    private void Drop()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);

            if (_mainCharacter.activeSelf || _isPapich)
            {
                if (_enemy.transform.TryGetComponent(out AbstractEnemy enemy))
                    enemy.IsDetected = true;
            }
        }

        _audioSource.Stop();
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if(Health <= 0f)
            Destroy();
    }

    public void Destroy()
    {
        _rb.useGravity = true;

        Vector3 randomForce = new Vector3(
            Random.Range(-2.5f, 2.5f),
            0,  
            Random.Range(-2.5f, 2.5f)
        );

        _rb.AddForce(randomForce, ForceMode.VelocityChange);
       
        foreach (Animator animator in _animators)
            Destroy(animator);

        foreach (MeshRenderer renderer in _meshRenderers)
            renderer.material = _destroyedMaterial;

        _audioSource.Stop();

        CancelInvoke();
    }
}
