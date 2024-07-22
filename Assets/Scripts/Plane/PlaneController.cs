using UnityEngine;
public class PlaneController : MonoBehaviour 
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _maxSpeed = 110f;
    [SerializeField] private float _minSpeed = 35f;

    [SerializeField] private float _force = 0.08f;

    [SerializeField] private float _pitchAngle = 70f;
    [SerializeField] private float _rollAngle = 45f;

    private float _pitchSmoothness;
    private float _rollSmoothness;

    private float _rollSpeed;
    private float _pitchSpeed;

    private float horizontalMovement;
    private float Amount = 120;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _flySpeed = 0f;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && _flySpeed < _maxSpeed)
            _flySpeed += _force;


        if (Input.GetKey(KeyCode.LeftShift) && _flySpeed > _minSpeed + 1f)
            _flySpeed -= _force;

        if (_flySpeed >= _minSpeed)
        {
            rb.useGravity = false;
            Movement();
        }

        transform.Translate(Vector3.forward * _flySpeed * Time.deltaTime);

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
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontalMovement += horizontal * Amount * _rollSpeed * Time.deltaTime;

        float targetVerticalMovement = Mathf.LerpAngle(0, _pitchAngle, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float verticalMovement = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, targetVerticalMovement, Time.deltaTime * _pitchSmoothness * _pitchSpeed);

        float targetRoll = Mathf.LerpAngle(0, _rollAngle, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);
        float roll = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, targetRoll, Time.deltaTime * _rollSmoothness);

        transform.localRotation = Quaternion.Euler(verticalMovement, horizontalMovement, roll);
    }
}