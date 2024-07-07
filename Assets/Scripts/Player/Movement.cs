using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Movement : MonoBehaviour, IService
{
    [SerializeField] private float _moveSpeed = 7.0f;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _slopeRayLength = 1.5f;

    private float _minPitch = 0.9f;
    private float _maxPitch = 1.1f;
    [SerializeField] private AudioSource _audioSource;

    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;

    public void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            SetMoveDirection();
            if (Input.GetButton("Jump"))
            {
                Jump();
            }

        }
        _moveDirection.y -= _gravity * Time.deltaTime;
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Slope();
    }

    private void SetMoveDirection()
    {
        if (Input.GetKey(KeyCode.W) && !_audioSource.isPlaying)
        {
            _audioSource.Play();
            ChangePitchRandomly();
        }
        if (Input.GetKey(KeyCode.S) && !_audioSource.isPlaying)
        {
            _audioSource.Play();
            ChangePitchRandomly();
        }
        if (Input.GetKey(KeyCode.A) && !_audioSource.isPlaying)
        {
            _audioSource.Play();
            ChangePitchRandomly();
        }
        if (Input.GetKey(KeyCode.D) && !_audioSource.isPlaying)
        {
            _audioSource.Play();
            ChangePitchRandomly();
        }
        else if (_moveDirection.magnitude <= 0.01f && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(horizontal, 0.0f, vertical);
        Direction = transform.TransformDirection(Direction);
        _moveDirection = Direction * _moveSpeed;
    }

    private void Jump()
    {
        _moveDirection.y = _jumpForce;
    }

    private void Slope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _slopeRayLength) == false)
            return;
    }
    void ChangePitchRandomly()
    {
        float randomPitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.pitch = randomPitch;
    }
}
