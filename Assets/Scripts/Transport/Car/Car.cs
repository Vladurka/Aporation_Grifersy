using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Car : AbstractTransport, IService
{
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSourceDrive;
    [SerializeField] private AudioSource _audioSourceIdle;
    [SerializeField] private AudioSource _audioSourceStart;
    private AudioListener _audioListener;

    [Header("Drive")]
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformBL;
    [SerializeField] private Transform _transformBR;

    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderBL;
    [SerializeField] private WheelCollider _colliderBR;

    [SerializeField] private float _maxAngle;

    private EventBus _eventBus;
    private Rigidbody _rb;

    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _camera.enabled = false;
        this.enabled = false;
        _rb = GetComponent<Rigidbody>();
        _audioListener = GetComponentInChildren<AudioListener>();
        _audioListener.enabled = false;
    }

    private void OnEnable()
    {
        _audioSourceStart.Play();
    }

    private void FixedUpdate()
    {
        Move();

        if (_rb.velocity.magnitude >= 0.1f)
        {
            if (!_audioSourceDrive.isPlaying)
            {
                _audioSourceDrive.volume = _rb.velocity.magnitude / 2;
                _audioSourceDrive.Play();
                _audioSourceIdle.Stop();
            }
        }

        if (_rb.velocity.magnitude < 0.1f)
        {
            if (!_audioSourceIdle.isPlaying)
            {
                _audioSourceIdle.Play();
                _audioSourceDrive.Stop();
            }
        }
    }
        

    protected override void Move()
    {
        _colliderFL.motorTorque = Input.GetAxis("Vertical") * _forwardSpeed;
        _colliderFR.motorTorque = Input.GetAxis("Vertical") * _forwardSpeed;


        if (Input.GetKey(KeyCode.Space))
        {
            _colliderFL.brakeTorque = 5000f;
            _colliderFR.brakeTorque = 5000f;
            _colliderBL.brakeTorque = 5000f;
            _colliderBR.brakeTorque = 5000f;
        }
        else
        {
            _colliderFL.brakeTorque = 0f;
            _colliderFR.brakeTorque = 0f;
            _colliderBL.brakeTorque = 0f;
            _colliderBR.brakeTorque = 0f;
        }

        _colliderFL.steerAngle = _maxAngle * Input.GetAxis("Horizontal");
        _colliderFR.steerAngle = _maxAngle * Input.GetAxis("Horizontal");

        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderBL, _transformBL);
        RotateWheel(_colliderBR, _transformBR);
    }
    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Exit();
        }
    }


    public override void Exit()
    {
        if (ConstSystem.CanExit)
        {
            _colliderFL.brakeTorque = 2000f;
            _colliderFR.brakeTorque = 2000f;
            _colliderBL.brakeTorque = 2000f;
            _colliderBR.brakeTorque = 2000f;

            _camera.enabled = false;
            _mainCharacter.transform.position = _spawnCharacter.position;
            _mainCharacter.SetActive(true);
            _eventBus.Invoke(new SetSpeedometer(false));
            this.enabled = false;
            ConstSystem.InTransport = false;
            ConstSystem.InCar = false;
            _audioListener.enabled = false;
        }
    }

    public override void Enter()
    {
        this.enabled = true;
        _camera.enabled = true;
        _eventBus.Invoke(new SetSpeedometer(true));
        _mainCharacter.SetActive(false);
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
        ConstSystem.InTransport = true;
        ConstSystem.InCar = true;
        _audioListener.enabled = true;
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
    }
}
