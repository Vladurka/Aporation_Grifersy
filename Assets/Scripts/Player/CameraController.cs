using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class CameraController : MonoBehaviour, IService
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensitivaty = 300f;
    [SerializeField] private Transform _playerBody;
    private Animator _camAnimator;
    private AudioSource _audioSource;

    private EventBus _eventBus;

    private float _rotationX = 0f;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ChangeSens>(GetSens, 1);

        Cursor.lockState = CursorLockMode.Locked;

        _camAnimator = Camera.main.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivaty * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivaty * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -55f, 90f);

        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);

        if(mouseY != 0.0f || mouseX != 0.0f)
        {
            if(!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            _camAnimator.SetBool("Walking", true);
        }
        else
            _camAnimator.SetBool("Walking", false);

    }

    private void GetSens(ChangeSens sens)
    {
        _sensitivaty = sens.Value;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<ChangeSens>(GetSens);
    }
}
