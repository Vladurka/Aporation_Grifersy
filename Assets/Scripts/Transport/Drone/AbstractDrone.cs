using UnityEngine;
using UnityEngine.UI;
using Game.SeniorEventBus;

public abstract class AbstractDrone : AbstractTransport
{
    [Header("Movement")]
    [SerializeField] protected float _rotationSpeed = 25f;
    [SerializeField] protected float _liftForce = 100f;
    [SerializeField] protected float _stopDuration = 1.5f;
    [SerializeField] protected float _targetForwardSpeed = 25f;
    [SerializeField] protected float _targetRotationSpeed = 25f;
    [SerializeField] protected float _maxDistance = 400f;

    [SerializeField] protected GameObject _gamerPrefab;
    protected GameObject _gamer;

    [HideInInspector] public Text TextDistance;
    [HideInInspector] public Text BatteryText;

    [HideInInspector] public Vector3 DroneSpawn;
    [HideInInspector] public Vector3 CharacterPos;

    protected float _stopTimer = 0f;
    protected float _currentVelocity;

    protected int _battery = 100;

    protected bool _isStopping = false;

    protected EventBus _eventBus;
    protected Rigidbody _rb;
    protected AudioSource _audioSource;
}
