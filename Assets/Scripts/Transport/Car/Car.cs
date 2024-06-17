using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class Car : AbstractTransport, IService
{
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

    [SerializeField] private Canvas _carCanvas;

    private EventBus _eventBus;

    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _carCanvas.enabled = false;
        _camera.enabled = false;
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        _colliderFL.motorTorque = Input.GetAxis("Vertical") * _forwardSpeed;
        _colliderFR.motorTorque = Input.GetAxis("Vertical") * _forwardSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            _colliderFL.brakeTorque = 3000f;
            _colliderFR.brakeTorque = 3000f;
            _colliderBL.brakeTorque = 3000f;
            _colliderBR.brakeTorque = 3000f;
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
        _colliderFL.brakeTorque = 2000f;
        _colliderFR.brakeTorque = 2000f;
        _colliderBL.brakeTorque = 2000f;
        _colliderBR.brakeTorque = 2000f;

        _camera.enabled = false;
        _mainCharacter.transform.position = _spawnCharacter.position;
        _mainCharacter.SetActive(true);
        _carCanvas.enabled = false;
        this.enabled = false;
    }

    public override void Enter()
    {
        this.enabled = true;
        _camera.enabled = true;
        _carCanvas.enabled = true;
        _mainCharacter.SetActive(false);
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
