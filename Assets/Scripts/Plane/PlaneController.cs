using UnityEngine;
public class PlaneController : MonoBehaviour 
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _maxSpeed = 120f;
    [SerializeField] private float _minSpeed = 70f;
    [SerializeField] private float _startSpeed = 50f;

    [SerializeField] private Transform _centerOfMass;

    [SerializeField] private float _force = 0.08f;

    [SerializeField] private float _pitchAngle = 70f;
    [SerializeField] private float _rollAngle = 45f;

    [SerializeField] private Transform _leftFlap;
    [SerializeField] private Transform _rightFlap;

    [SerializeField] private Transform _leftWing;
    [SerializeField] private Transform _rightWing;
    [SerializeField] private float _wingSpeed = 2f;

    [SerializeField] private float _flapSpeed = 15f;
    [SerializeField] private float _flapAngle = 20f;

    private float _pitchSmoothness;
    private float _rollSmoothness;

    private float _rollSpeed;
    private float _pitchSpeed;

    private float _horizontalMovement;
    private float _amount = 120;

    private bool _isClosed = false;

    private Rigidbody rb;
    private Animator _animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _flySpeed = 0f;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (_flySpeed >= _startSpeed)
        {
            rb.useGravity = false;
            Movement();
        }

        if (Input.GetKey(KeyCode.Space) && _flySpeed < _maxSpeed)
            _flySpeed += _force;


        if (Input.GetKey(KeyCode.Space) && _flySpeed > 80)
        {
            if(_leftWing.localRotation.y >= -45 && _rightWing.localRotation.y <= 45)
            {
                Quaternion leftWingTargetRotation = Quaternion.Euler(-180, -45, 0);
                Quaternion rightWingTargetRotation = Quaternion.Euler(0, 45, 0);

                _leftWing.localRotation = Quaternion.Lerp(_leftWing.localRotation, leftWingTargetRotation, Time.deltaTime * _wingSpeed);
                _rightWing.localRotation = Quaternion.Lerp(_rightWing.localRotation, rightWingTargetRotation, Time.deltaTime * _wingSpeed);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_leftWing.localRotation.y <= -12 && _rightWing.localRotation.y >= 12)
            {
                Quaternion leftWingTargetRotation = Quaternion.Euler(-180, -12, 0);
                Quaternion rightWingTargetRotation = Quaternion.Euler(0, 12, 0);

                _leftWing.localRotation = Quaternion.Lerp(_leftWing.localRotation, leftWingTargetRotation, Time.deltaTime * _wingSpeed);
                _rightWing.localRotation = Quaternion.Lerp(_rightWing.localRotation, rightWingTargetRotation, Time.deltaTime * _wingSpeed);
            }
        }


        if (Input.GetKey(KeyCode.LeftShift) && _flySpeed > _minSpeed + 1f)
            _flySpeed -= _force;

        transform.Translate(Vector3.forward * _flySpeed * Time.deltaTime);

    }

    private void Update()
    {
        if (_flySpeed <= 70f)
        {
            _pitchSmoothness = _flySpeed / 20f;
            _rollSmoothness = _flySpeed / 20f;
            _rollSpeed = _rollSmoothness / 10f;
            _pitchSpeed = _pitchSmoothness / 20f;
        }

        else if (_flySpeed > 70f)
        {
            _pitchSmoothness = 3.5f;
            _rollSmoothness = 3.5f;
            _rollSpeed = 0.35f;
            _pitchSpeed = 0.175f;
        }

        if (_flySpeed >= 60f && !_isClosed)
        {
            _animator.SetTrigger("CloseWheels");
            _isClosed = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(-_flapAngle, 0, 0);
            Quaternion rightFlapTargetRotation = Quaternion.Euler(-200, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(_flapAngle, 0, 0);
            Quaternion rightFlapTargetRotation = Quaternion.Euler(-160, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }

        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            Quaternion leftFlapTargetRotation = Quaternion.Euler(0, 0, 0);
            Quaternion  rightFlapTargetRotation = Quaternion.Euler(-180, 0, 0);

            _leftFlap.localRotation = Quaternion.Lerp(_leftFlap.localRotation, leftFlapTargetRotation, Time.deltaTime * _flapSpeed);
            _rightFlap.localRotation = Quaternion.Lerp(_rightFlap.localRotation, rightFlapTargetRotation, Time.deltaTime * _flapSpeed);
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _horizontalMovement += horizontal * _amount * _rollSpeed * Time.deltaTime;

        float targetVerticalMovement = Mathf.LerpAngle(0, _pitchAngle, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float verticalMovement = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, targetVerticalMovement, Time.deltaTime * _pitchSmoothness * _pitchSpeed);

        float targetRoll = Mathf.LerpAngle(0, _rollAngle, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);
        float roll = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, targetRoll, Time.deltaTime * _rollSmoothness);

        transform.localRotation = Quaternion.Euler(verticalMovement, _horizontalMovement, roll);
    }
}