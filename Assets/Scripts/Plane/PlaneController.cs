using UnityEngine;
public class PlaneController : MonoBehaviour 
{
    [SerializeField] private float _flySpeed;
    [SerializeField] private float _maxSpeed = 50f;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _force = 0.01f;
    private float _pitchSmoothness;
    private float _rollSmoothness;

    private float horizontalMovement;
    private float Amount = 120;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _flySpeed = 0f;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.F) && _flySpeed < _maxSpeed)
            _flySpeed += _force;


        if (Input.GetKey(KeyCode.G) && _flySpeed > 0)
            _flySpeed -= _force;

        if (_flySpeed >= _minSpeed)
        {
            rb.useGravity = false;
            Movement();
        }

        if(_flySpeed <= _minSpeed)
            rb.useGravity = true;

        transform.Translate(Vector3.forward * _flySpeed * Time.deltaTime);

        _pitchSmoothness = _flySpeed / 20;
        _rollSmoothness = _flySpeed / 20;
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontalMovement += horizontal * Amount * Time.deltaTime;

        float targetVerticalMovement = Mathf.LerpAngle(0, 45, Mathf.Abs(vertical)) * Mathf.Sign(vertical);
        float verticalMovement = Mathf.LerpAngle(transform.localRotation.eulerAngles.x, targetVerticalMovement, Time.deltaTime * _pitchSmoothness);

        float targetRoll = Mathf.LerpAngle(0, 45, Mathf.Abs(horizontal)) * -Mathf.Sign(horizontal);
        float roll = Mathf.LerpAngle(transform.localRotation.eulerAngles.z, targetRoll, Time.deltaTime * _rollSmoothness);

        transform.localRotation = Quaternion.Euler(verticalMovement, horizontalMovement, roll);
    }
}