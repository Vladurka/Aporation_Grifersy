using UnityEngine;
using UnityEngine.UI;
public class PlaneController : MonoBehaviour, IService
{
    public float FlySpeed;
    [SerializeField] private float _maxSpeed = 120f;
    [SerializeField] private float _minSpeed = 70f;
    [SerializeField] private float _startSpeed = 50f;

    public float Force = 0.08f;

    [SerializeField] private float _pitchAngle = 70f;
    [SerializeField] private float _rollAngle = 45f;

    [SerializeField] private Transform _leftFlap;
    [SerializeField] private Transform _rightFlap;

    [SerializeField] private Transform _leftWing;
    [SerializeField] private Transform _rightWing;
    [SerializeField] private float _wingSpeed = 2f;

    [SerializeField] private float _flapSpeed = 15f;
    [SerializeField] private float _flapAngle = 20f;

    [SerializeField] private Text _speedText;
    [SerializeField] private Text _forceText;

    [SerializeField] private AudioSource _engineSound;

    private float _pitchSmoothness;
    private float _rollSmoothness;

    private float _rollSpeed;
    private float _pitchSpeed;

    private float _horizontalMovement;
    private float _amount = 120;

    public bool IsClosed = false;
    public bool CanFly = false;
    public bool IsStarted = false;
    public bool IsSet = false;

    public Rigidbody Rb;
    private Animator _animator;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        FlySpeed = 0f;
        Rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (FlySpeed >= _startSpeed)
        {
            if (!IsSet)
            {
                Rb.useGravity = false;
                IsSet = true;
                CanFly = true;
                IsStarted = true;
            }
        }

        if (CanFly)
        {
            Movement();
        }

        if (Input.GetKey(KeyCode.Space) && FlySpeed < _maxSpeed)
        {
            FlySpeed += Force;
            if (_engineSound.volume <= 0.05f)
                _engineSound.volume += 0.00001f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && FlySpeed > _minSpeed + 1f)
        {
            FlySpeed -= Force;
            if (_engineSound.volume >= 0.01f)
                _engineSound.volume -= 0.00001f;
        }


        if (Input.GetKey(KeyCode.Space) && FlySpeed > 100f)
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

        transform.Translate(Vector3.forward * FlySpeed * Time.deltaTime);

    }

    private void Update()
    {
        int convertedSpeed = (int)FlySpeed * 3;
        _speedText.text = convertedSpeed.ToString() + " km/h";

        int convertedPower = (int)FlySpeed / 4;
        _forceText.text = convertedPower.ToString() + " %";

        if (FlySpeed <= _minSpeed)
        {
            _pitchSmoothness = FlySpeed / 20f;
            _rollSmoothness = FlySpeed / 20f;
            _rollSpeed = _rollSmoothness / 10f;
            _pitchSpeed = _pitchSmoothness / 20f;
        }

        else if (FlySpeed > _minSpeed)
        {
            _pitchSmoothness = 3.5f;
            _rollSmoothness = 3.5f;
            _rollSpeed = 0.35f;
            _pitchSpeed = 0.175f;
        }

        if (Input.GetKeyDown(KeyCode.C) && FlySpeed >=  _minSpeed)
        {
            if (!IsClosed)
            {
                _animator.SetBool("CloseWheels",  true);
                IsClosed = true;
            }

            else
            {
                _animator.SetBool("CloseWheels", false);
                IsClosed = false;
            }
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