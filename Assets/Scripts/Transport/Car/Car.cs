using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Car : AbstractTransport, IService
{
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSourceDrive;
    [SerializeField] private AudioSource _audioSourceIdle;
    [SerializeField] private AudioSource _audioSourceStart;

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

    [SerializeField] private GameObject _driver;

    private EventBus _eventBus;
    private Rigidbody _rb;

    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new SetSpeedometer(false));

        _rb = GetComponent<Rigidbody>();

        _camera.enabled = false;
        this.enabled = false;

        ConstSystem.CanExit = true;
        _driver.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Exit();
    }

    private void FixedUpdate()
    {
        Move();

        if (_rb.velocity.magnitude >= 2f)
        {
            if (!_audioSourceDrive.isPlaying)
            {
                _audioSourceDrive.Play();
                _audioSourceIdle.Stop();
            }
        }

        if (_rb.velocity.magnitude < 2f)
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

        AdjustFriction(_colliderFL);
        AdjustFriction(_colliderFR);
        AdjustFriction(_colliderBL);
        AdjustFriction(_colliderBR);

        ApplyAntiRollBar(_colliderFL, _colliderFR);
        ApplyAntiRollBar(_colliderBL, _colliderBR);
    }
    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void AdjustFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.extremumSlip = 0.4f;   
        forwardFriction.extremumValue = 1.0f;  
        forwardFriction.stiffness = 2.0f;

        sidewaysFriction.extremumSlip = 0.2f;
        sidewaysFriction.extremumValue = 1.0f;
        sidewaysFriction.stiffness = 2.5f;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void ApplyAntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1f;

        bool groundedL = leftWheel.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        bool groundedR = rightWheel.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        float antiRollForce = (travelL - travelR) * 100000f;

        if (groundedL)
            _rb.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);

        if (groundedR)
            _rb.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
    }

    public override void Enter()
    {
        this.enabled = true;
        _audioSourceStart.Play();
        _camera.enabled = true;
        _eventBus.Invoke(new SetSpeedometer(true));
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
        _eventBus.Invoke(new EnterTransport());
        ConstSystem.InTransport = true;
        ConstSystem.InCar = true;
        _driver.SetActive(true);
    }

    public override void Exit()
    {
        if (ConstSystem.CanExit)
        {
            _colliderFL.brakeTorque = 2000f;
            _colliderFR.brakeTorque = 2000f;
            _colliderBL.brakeTorque = 2000f;
            _colliderBR.brakeTorque = 2000f;

            _audioSourceDrive.Stop();
            _audioSourceIdle.Stop();
            _audioSourceStart.Stop();

            _camera.enabled = false;
            _eventBus.Invoke(new SetSpeedometer(false));
            _eventBus.Invoke(new ExitTransport(_spawnCharacter));
            this.enabled = false;
            ConstSystem.InTransport = false;
            ConstSystem.InCar = false;
            _driver.SetActive(false);
        }
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
    }
}
