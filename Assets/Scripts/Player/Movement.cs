using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Movement : MonoBehaviour, IService
{
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _slopeRayLength = 1.5f;
    [SerializeField] private AudioSource _cameraAudioSource;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _animator;

    private float _minPitch = 0.9f;
    private float _maxPitch = 1.1f;

    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;

    public float NormalSpeed = 5.0f;
    public float ScopeSpeed = 3.0f;
    public float Speed = 5.0f;

    public bool InScope;

    public void Init()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            SetMoveDirection();

            if (Input.GetButton("Jump"))
                Jump();
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 Direction = new Vector3(horizontal, 0.0f, vertical);
        Direction = transform.TransformDirection(Direction);
        _moveDirection = Direction * Speed;

        if (vertical != 0.0f || horizontal != 0.0f)
        {
            if (_controller.isGrounded)
            {
                if (!_audioSource.isPlaying && InScope)
                {
                    _audioSource.Play();
                    _cameraAudioSource.Stop();
                }

                if (!_cameraAudioSource.isPlaying && !InScope)
                {
                    _audioSource.Stop();
                    _cameraAudioSource.Play();
                }
            }

            else
            {
                _audioSource.Stop();
                _cameraAudioSource.Stop();
            }

            _animator.SetBool("Walking", true);
        }

        else
        {
            _animator.SetBool("Walking", false);

            if (_audioSource.isPlaying)
                _audioSource.Stop();

            if (_cameraAudioSource.isPlaying)
                _cameraAudioSource.Stop();
        }
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
    private void ChangePitchRandomly()
    {
        float randomPitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.pitch = randomPitch;
    }
}
