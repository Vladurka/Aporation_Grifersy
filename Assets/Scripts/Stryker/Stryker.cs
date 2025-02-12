using UnityEngine;

public class Stryker : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSourceDrive;
    [SerializeField] private AudioSource _audioSourceIdle;

    [Header("Drive")]
    [SerializeField] private Transform _transformFR;
    [SerializeField] private Transform _transformFMR;
    [SerializeField] private Transform _transformBMR;
    [SerializeField] private Transform _transformBR;
    [SerializeField] private Transform _transformFL;
    [SerializeField] private Transform _transformFML;
    [SerializeField] private Transform _transformBML;
    [SerializeField] private Transform _transformBL;

    [SerializeField] private WheelCollider _colliderFR;
    [SerializeField] private WheelCollider _colliderFMR;
    [SerializeField] private WheelCollider _colliderBMR;
    [SerializeField] private WheelCollider _colliderBR;
    [SerializeField] private WheelCollider _colliderFL;
    [SerializeField] private WheelCollider _colliderFML;
    [SerializeField] private WheelCollider _colliderBML;
    [SerializeField] private WheelCollider _colliderBL;

    [Header("Stats")]
    [SerializeField] private float _forwardSpeed = 750f;
    [SerializeField] private float _maxAngle = 30f;
    [SerializeField] private float _brakeTorque = 5000f;
    [SerializeField] private float _antiRoll = 5000f;

    private bool _crabMode = false;
    private bool _frontWheelDriveMode = true;

    private Rigidbody _rb;

    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_crabMode)
            {
                _crabMode = false;
                _forwardSpeed = _forwardSpeed * 3f;
            }

            else
            {
                _crabMode = true;
                _forwardSpeed = _forwardSpeed / 3f;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_frontWheelDriveMode)
                _frontWheelDriveMode = false;

            else
                _frontWheelDriveMode = true;
        }

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

    private void FixedUpdate()
    {
        Move();
        AntiRollBar(_colliderFR, _colliderFL);
        AntiRollBar(_colliderFMR, _colliderFML);
        AntiRollBar(_colliderBMR, _colliderBML);
        AntiRollBar(_colliderBR, _colliderBL);
    }


    protected void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _colliderFR.motorTorque = vertical * _forwardSpeed;
        _colliderFL.motorTorque = vertical * _forwardSpeed;
        _colliderFMR.motorTorque = vertical * _forwardSpeed;
        _colliderFML.motorTorque = vertical * _forwardSpeed;
        _colliderBR.motorTorque = vertical * _forwardSpeed;
        _colliderBL.motorTorque = vertical * _forwardSpeed;
        _colliderBMR.motorTorque = vertical * _forwardSpeed;
        _colliderBML.motorTorque = vertical * _forwardSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            _colliderFR.brakeTorque = _brakeTorque;
            _colliderFMR.brakeTorque = _brakeTorque;
            _colliderBMR.brakeTorque = _brakeTorque;
            _colliderBR.brakeTorque = _brakeTorque;

            _colliderFL.brakeTorque = _brakeTorque;
            _colliderFML.brakeTorque = _brakeTorque;
            _colliderBML.brakeTorque = _brakeTorque;
            _colliderBL.brakeTorque = _brakeTorque;
        }
        else
        {
            _colliderFR.brakeTorque = 0f;
            _colliderFMR.brakeTorque = 0f;
            _colliderBMR.brakeTorque = 0f;
            _colliderBR.brakeTorque = 0f;

            _colliderFL.brakeTorque = 0f;
            _colliderFML.brakeTorque = 0f;
            _colliderBML.brakeTorque = 0f;
            _colliderBL.brakeTorque = 0f;
        }


        if (!_crabMode)
        {
            if (_frontWheelDriveMode)
            {
                _colliderFR.steerAngle = _maxAngle * horizontal;
                _colliderFL.steerAngle = _maxAngle * horizontal;
                _colliderFMR.steerAngle = _maxAngle * horizontal;
                _colliderFML.steerAngle = _maxAngle * horizontal;
            }

            if(!_frontWheelDriveMode)
            {
                _colliderBR.steerAngle = _maxAngle * horizontal;
                _colliderBL.steerAngle = _maxAngle * horizontal;
                _colliderBMR.steerAngle = _maxAngle * horizontal;
                _colliderBML.steerAngle = _maxAngle * horizontal;
            }
        }

        if(_crabMode)
        {
            _colliderFR.steerAngle = _maxAngle * horizontal;
            _colliderFL.steerAngle = _maxAngle * horizontal;
            _colliderFMR.steerAngle = _maxAngle * horizontal;
            _colliderFML.steerAngle = _maxAngle * horizontal;
            _colliderBR.steerAngle = _maxAngle * horizontal;
            _colliderBL.steerAngle = _maxAngle * horizontal;
            _colliderBMR.steerAngle = _maxAngle * horizontal;
            _colliderBML.steerAngle = _maxAngle * horizontal;
        }



        RotateWheel(_colliderFR, _transformFR);
        RotateWheel(_colliderFMR, _transformFMR);
        RotateWheel(_colliderBMR, _transformBMR);
        RotateWheel(_colliderBR, _transformBR);
        RotateWheel(_colliderFL, _transformFL);
        RotateWheel(_colliderFML, _transformFML);
        RotateWheel(_colliderBML, _transformBML);
        RotateWheel(_colliderBL, _transformBL);
    }
    private void AntiRollBar(WheelCollider wheelL, WheelCollider wheelR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = wheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;

        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * _antiRoll;

        if (groundedL)
            GetComponent<Rigidbody>().AddForceAtPosition(wheelL.transform.up * -antiRollForce, wheelL.transform.position);

        if (groundedR)
            GetComponent<Rigidbody>().AddForceAtPosition(wheelR.transform.up * antiRollForce, wheelR.transform.position);
    }

    private void RotateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        transform.rotation = rotation;
        transform.position = position;
    }
}

