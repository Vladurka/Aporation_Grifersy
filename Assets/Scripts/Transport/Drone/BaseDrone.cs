using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;
using System.Collections;

public class BaseDrone : AbstractDrone
{
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _camera.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        ConstSystem.CanExit = true;
        StartCoroutine(Timer());
        Enter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Exit();

        float distance = Vector3.Distance(transform.position, DroneSpawn);
        int intDistance = (int)distance;
        TextDistance.text = $"{intDistance}m";

        if (distance >= _maxDistance)
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

    private IEnumerator Timer()
    {
        _battery--;
        BatteryText.text = $"{_battery}%";

        if (_battery <= 0)
        {
            Exit();
            yield break;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer());
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
        _gamer = Instantiate(_gamerPrefab, CharacterPos, Quaternion.Euler(-90, 0, 0));
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
            Destroy(_gamer);
            Destroy(gameObject);
        }
    }

    public override void TransportReset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
    }
}
