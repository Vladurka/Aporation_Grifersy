using UnityEngine;

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

    private Rigidbody _rb;

    private Animator _animator;

    public override void Init()
    {
        _camera.enabled = false;

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        this.enabled = false;
    }

    private void OnEnable()
    {
        _animator.SetBool("Fly", true);
        _rb.useGravity = false;
    }

    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.E))
        {
            Exit();
        }
    }

    protected override void Move()
    {
        if (_canMove == true)
        {
            _forwardSpeed = 10f;
            _rotationSpeed = 25f;
        }

        if (_canMove == false)
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
    }

    public override void Exit()
    {
        _camera.enabled = false;
        _mainCharacter.transform.position = _spawnCharacter.position;
        _mainCharacter.SetActive(true);
        Invoke("UseGravity", 2f);
        this.enabled = false;
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void UseGravity()
    {
        _rb.useGravity = true; 
        _animator.SetBool("Fly", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
            _canMove = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        _canMove = true;
    }
}
