using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;
using UnityEngine.UI;

public class Drone : AbstractTransport
{
    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 25f;
    [SerializeField] private float _liftForce = 100f;
    [SerializeField] private float _stopDuration = 1.5f;
    [SerializeField] private float _targetForwardSpeed = 25f;
    [SerializeField] private float _targetRotationSpeed = 25f;

    public Text TextDistance;
    public Vector3 DroneSpawn;

    private float _stopTimer = 0f;
    private float _currentVelocity;

    private bool _isStopping = false;

    private EventBus _eventBus;
    private Rigidbody _rb;
    private AudioSource _audioSource;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _camera.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        ConstSystem.CanExit = true;

        Invoke("Enter", 0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Exit();

        float distance = Vector3.Distance(transform.position, DroneSpawn);
        int intDistance = (int)distance;
        TextDistance.text = intDistance.ToString() + "m";

        if (distance >= 400f)
            Exit();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
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
        _camera.enabled = true;
        _eventBus.Invoke(new SetCurrentBullets(false));
        _eventBus.Invoke(new SetTotalBullets(false));
        _eventBus.Invoke(new SetDronePanel(true));
        ConstSystem.InTransport = true;
        ConstSystem.InDrone = true;
        _audioSource.Play();
        GamePanel.SetActive(false);
        MainCharacter.SetActive(false);
    }

    public override void Exit()
    {
        if (ConstSystem.CanExit)
        {
            _eventBus.Invoke(new SetDronePanel(false));
            _camera.enabled = false;
            MainCharacter.SetActive(true);
            ConstSystem.InTransport = false;
            ConstSystem.InDrone = false;
            _audioSource.Stop();
            GamePanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
    }
}
