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

        this.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Exit();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (_canMove)
        {
            _forwardSpeed = 10f;
            _rotationSpeed = 25f;
        }

        if (!_canMove)
        {
            _forwardSpeed = 0f;
            _rotationSpeed = 0f;
        }


        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up * moveHorizontal * _rotationSpeed * Time.deltaTime);

        Vector3 forwardVelocity = transform.forward * moveVertical * _forwardSpeed;
        forwardVelocity.y = _rb.velocity.y;
        _rb.velocity = forwardVelocity;

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
    }

    public override void Enter()
    {
        this.enabled = true;
        _camera.enabled = true;
        _mainCharacter.SetActive(false);
        _animator.SetBool("Fly", true);
        _rb.useGravity = false;
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
        ConstSystem.InTransport = true;
        _audioSource.Play();
        _audioListener.enabled = true;
    }

    public override void Exit()
    {
        if (ConstSystem.CanExit)
        {
            _camera.enabled = false;
            _mainCharacter.transform.position = _spawnCharacter.position;
            _mainCharacter.SetActive(true);
            Invoke("UseGravity", 2f);
            this.enabled = false;
            ConstSystem.InTransport = false;
            _audioSource.Stop();
            _audioListener.enabled = false;
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
