using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class TransportHealth : MonoBehaviour, ITargetHealth
{
    public bool IsArmored { get; set; }
    public float Health { get; set; } = 100f;

    [SerializeField] private bool _isArmored = true;   

    private AudioSource[] _audioSource;
    private EventBus _eventBus;

    private void Start()
    {
        _audioSource = GetComponentsInChildren<AudioSource>();
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        IsArmored = _isArmored;
    }

    public void Destroy()
    {
        Cursor.lockState = CursorLockMode.None;
        _eventBus.Invoke(new SetDie());
        if (ConstSystem.InTransport)
        {
            foreach (AudioSource sourse in _audioSource)
                sourse.Stop();

            ConstSystem.CanSave = false;
            Destroy(gameObject);
        }
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Invoke("Die", 0.5f);
            Health = 0;
        }
    }
}
