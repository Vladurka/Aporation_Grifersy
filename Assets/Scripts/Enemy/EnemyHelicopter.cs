using Game.Enemy;
using UnityEngine;

public class EnemyHelicopter : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _enemy;

    private AudioSource _audioSource;
    private Animator _animator;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("Fly");
        Invoke("Drop", 35f);
        _audioSource.Play();
    }

    private void Drop()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);

            if(_enemy.transform.TryGetComponent(out AbstractEnemy enemy))
                enemy.IsDetected = true;
        }

        _audioSource.Stop();
    }
}
