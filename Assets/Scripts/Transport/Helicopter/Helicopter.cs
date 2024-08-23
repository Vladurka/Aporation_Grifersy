using Game.SeniorEventBus.Signals;
using UnityEngine;
using Game.SeniorEventBus;

[RequireComponent(typeof(Rigidbody))]
public class Helicopter : AbstractTransport, IService
{
    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 25f;
    [SerializeField] private float _liftForce = 100f;
    [SerializeField] private float _stopDuration = 1.5f;

    [SerializeField] private float _targetForwardSpeed = 25f;
    [SerializeField] private float _targetRotationSpeed = 25f;

    private float _stopTimer = 0f;
    private float _currentVelocity;

    private bool _isStopping = false;
    private bool _canMove = true;

    private EventBus _eventBus;
    private AudioListener _audioListener;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private Animator _animator;

    public override void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _camera.enabled = false;
        _audioListener = GetComponent<AudioListener>();
        _audioListener.enabled = false;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        ConstSystem.CanExit = true;

        this.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Exit();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (_canMove)
        {
            _targetForwardSpeed = 25f;
            _targetRotationSpeed = 25f;
        }
        else
        {
            _targetForwardSpeed = 0f;
            _targetRotationSpeed = 0f;
        }

        _forwardSpeed = Mathf.Lerp(_forwardSpeed, _targetForwardSpeed, Time.deltaTime);
        _rotationSpeed = Mathf.Lerp(_rotationSpeed, _targetRotationSpeed, Time.deltaTime);

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up * moveHorizontal * _rotationSpeed * Time.deltaTime);

        Vector3 forwardVelocity = transform.forward * moveVertical * _forwardSpeed;
        forwardVelocity.y = _rb.velocity.y;
        _rb.velocity = forwardVelocity;

        float tiltAmountZ = moveHorizontal * -30f; 
        float tiltAmountX = moveVertical * 10f; 

        if (moveVertical == 0 && moveHorizontal == 0)
        {
            tiltAmountX += Mathf.Sin(Time.time * 1.5f) * 2f;  
            tiltAmountZ += Mathf.Sin(Time.time * 2f) * 2f; 
        }

        Quaternion targetTilt = Quaternion.Euler(tiltAmountX, transform.rotation.eulerAngles.y, tiltAmountZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, Time.deltaTime * 2f);

        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _liftForce * Time.deltaTime);
            _isStopping = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _rb.AddForce(Vector3.down * _liftForce * Time.deltaTime);
            _isStopping = false;
        }
        else
        {
            if (!_isStopping)
            {
                _isStopping = true;
                _stopTimer = 0f;
                _currentVelocity = _rb.velocity.y;
            }

            _stopTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_stopTimer / _stopDuration);
            float targetVelocity = Mathf.Lerp(_currentVelocity, 0f, t);
            _rb.velocity = new Vector3(_rb.velocity.x, targetVelocity, _rb.velocity.z);
        }

        Vector3 angularVelocity = _rb.angularVelocity;
        angularVelocity.x *= 0.95f; 
        angularVelocity.z *= 0.95f; 
        _rb.angularVelocity = angularVelocity;

        Quaternion currentRotation = transform.rotation;
        Quaternion stabilizedRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(currentRotation, stabilizedRotation, Time.deltaTime * 5f);
    }


    public override void Enter()
    {
        this.enabled = true;
        _camera.enabled = true;
        MainCharacter.SetActive(false);
        _animator.SetBool("Fly", true);
        _rb.useGravity = false;
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
        ConstSystem.InTransport = true;
        _audioSource.Play();
        _audioListener.enabled = true;
        GamePanel.SetActive(false);
    }

    public override void Exit()
    {
        if (ConstSystem.CanExit)
        {
            _camera.enabled = false;
            MainCharacter.transform.position = _spawnCharacter.position;
            MainCharacter.SetActive(true);
            Invoke("UseGravity", 2f);
            this.enabled = false;
            ConstSystem.InTransport = false;
            _audioSource.Stop();
            _audioListener.enabled = false;
            GamePanel.SetActive(true);
        }
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
    }

    private void UseGravity()
    {
        _rb.useGravity = true; 
        _animator.SetBool("Fly", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            _canMove = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        _canMove = true;
    }
}
