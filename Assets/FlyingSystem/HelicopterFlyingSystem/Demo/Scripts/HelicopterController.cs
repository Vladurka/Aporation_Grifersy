using UnityEngine;

using FlyingSystem;

public class HelicopterController : MonoBehaviour
{
    public Transform springArmTransform;
    public Camera characterCamera;

    public Rigidbody rootRigidbody;

    public Transform topRotorTransform;
    public Transform tailRotorTransform;

    private HelicopterFlyingSystem helicopterFlyingSystem;

    private AudioSource audioSource;

    public bool activated = false;

    public float cameraSpeed = 300.0f;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;

    private bool draggingMouse = false;

    private float accumulatedDeltaMousePositionX, accumulatedDeltaMousePositionY;

    [Header("Mobile")]
    public Joystick joystick;
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        if (activated)
            Activate();

        helicopterFlyingSystem = this.GetComponent<HelicopterFlyingSystem>();

        screenCenterX = screenCenterX = Screen.width / 2.0f;
    }

    void Update()
    {
        if (activated)
        {
            if (!mobileInputControl)
            {
                if (!draggingMouse)
                    PCCameraControlLogic();

                PCInputControlLogic();
            }
            else
            {
                MobileCameraControlLogic();
                MobileInputControlLogic();
            }

            topRotorTransform.Rotate(Vector3.forward * 1280.0f * Time.deltaTime);
            tailRotorTransform.Rotate(Vector3.forward * 1280.0f * Time.deltaTime);
        }
    }

    public void Activate()
    {
        activated = true;
        characterCamera.enabled = true;
        characterCamera.GetComponent<AudioListener>().enabled = true;

        audioSource.Play();
    }

    public void Deactivate()
    {
        activated = false;
        characterCamera.enabled = false;
        characterCamera.GetComponent<AudioListener>().enabled = false;

        audioSource.Stop();
    }

    void PCCameraControlLogic()
    {
        springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
    }

    void MobileCameraControlLogic()
    {
        // Temporarily use mouse to simulate the touch
        if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
        {
            springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
            springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
        }

        // Only detects on mobile devices
        if (Input.touchCount > 0)
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).position.x > screenCenterX && Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.x * Time.deltaTime);
                    springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.y * Time.deltaTime);
                }
            }
        }
    }

    void PCInputControlLogic()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            TakeOffOrLand();

        // Hold down to turn left / right
        if (Input.GetKey(KeyCode.A))
        {
            TakeOff();
            helicopterFlyingSystem.AddYawInput(-1.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TakeOff();
            helicopterFlyingSystem.AddYawInput(1.0f);
        }

        // Hold down to ascend / descend
        if (Input.GetKey(KeyCode.W))
        {
            TakeOff();
            Ascend();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            TakeOff();
            Descend();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            helicopterFlyingSystem.VerticalSlowDown();

        // Hold down mouse left button and drag to move
        if (Input.GetMouseButtonDown(0))
            draggingMouse = true;

        if (draggingMouse)
        {
            accumulatedDeltaMousePositionX += Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0f, 1.0f);
            accumulatedDeltaMousePositionY += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1.0f, 1.0f);

            helicopterFlyingSystem.AddHorizontalInput(new Vector2(accumulatedDeltaMousePositionX, accumulatedDeltaMousePositionY));

            TakeOff();
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingMouse = false;
            accumulatedDeltaMousePositionX = 0.0f;
            accumulatedDeltaMousePositionY = 0.0f;
            helicopterFlyingSystem.StopYawInput();
        }

        // Boost on / off
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            Boost();
    }

    void MobileInputControlLogic()
    {
        if (joystick != null)
        {
            if (joystick.isMoving)
            {
                draggingMouse = true;
                accumulatedDeltaMousePositionX += joystick.inputAxisX;
                accumulatedDeltaMousePositionY += joystick.inputAxisY;

                helicopterFlyingSystem.AddHorizontalInput(new Vector2(accumulatedDeltaMousePositionX, accumulatedDeltaMousePositionY));

                TakeOff();
            }
            else
            {
                if (draggingMouse)
                {
                    draggingMouse = false;
                    accumulatedDeltaMousePositionX = 0.0f;
                    accumulatedDeltaMousePositionY = 0.0f;
                    helicopterFlyingSystem.StopYawInput();
                }
            }
        }
    }

    public void TakeOffOrLand()
    {
        if (helicopterFlyingSystem.inAir)
        {
            helicopterFlyingSystem.Land();
            takeOff = helicopterFlyingSystem.inAir;

            rootRigidbody.useGravity = true;
            rootRigidbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            TakeOff();
        }
    }

    void TakeOff()
    {
        if (!helicopterFlyingSystem.inAir)
        {
            helicopterFlyingSystem.TakeOff();
            takeOff = helicopterFlyingSystem.inAir;

            rootRigidbody.useGravity = false;
            rootRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void Boost()
    {
        helicopterFlyingSystem.boosting = !helicopterFlyingSystem.boosting;
        boosting = helicopterFlyingSystem.boosting;
    }

    public void MobileTurnLeft()
    {
        TakeOff();
        helicopterFlyingSystem.AddYawInput(-1.0f);
    }

    public void MobileTurnRight()
    {
        TakeOff();
        helicopterFlyingSystem.AddYawInput(1.0f);
    }

    public void Ascend()
    {
        TakeOff();
        helicopterFlyingSystem.AddVerticalInput(1.0f);
    }

    public void Descend()
    {
        TakeOff();
        helicopterFlyingSystem.AddVerticalInput(-1.0f);
    }

    public void MobileStopAscendOrDescend()
    {
        helicopterFlyingSystem.VerticalSlowDown();
    }

    public float GetFlyingSpeed()
    {
        return helicopterFlyingSystem.horizontalFlyingSpeed;
    }

    public float GetPowerPercentage()
    {
        return helicopterFlyingSystem.powerPercentage;
    }

    public float GetWeightPercentage()
    {
        return helicopterFlyingSystem.weightPercentage;
    }
}