using UnityEngine;

using FlyingSystem;

public class FlyingCarController : MonoBehaviour
{
    public Transform springArmTransform;
    public Camera characterCamera;

    public Rigidbody rootRigidbody;

    private FlyingVehicleFlyingSystem flyingVehicleFlyingSystem;

    public bool activated = false;

    public float cameraSpeed = 300.0f;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;

    [Header("Mobile")]
    public Joystick joystick;
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    private float targetSpringArmRotationX, targetSpringArmRotationY;

    void Start()
    {
        flyingVehicleFlyingSystem = this.GetComponent<FlyingVehicleFlyingSystem>();

        screenCenterX = screenCenterX = Screen.width / 2.0f;

        if (activated)
            Activate();
    }

    void Update()
    {
        if (activated)
        {
            if (!mobileInputControl)
            {
                PCCameraControlLogic();
                PCInputControlLogic();
            }
            else
            {
                MobileCameraControlLogic();
                MobileInputControlLogic();
            }
        }
    }

    public void Activate()
    {
        activated = true;
        characterCamera.enabled = true;
        characterCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        characterCamera.enabled = false;
        characterCamera.GetComponent<AudioListener>().enabled = false;
    }

    void PCCameraControlLogic()
    {
        targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
        targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;

        springArmTransform.rotation = Quaternion.Euler(targetSpringArmRotationX, targetSpringArmRotationY, 0.0f);
    }

    void MobileCameraControlLogic()
    {
        // Temporarily use mouse to simulate the touch
        if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
        {
            springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
            springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
        }

        // Only for mobile devices(uncomment the following and test on physical mobile devices)
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

        if (Input.GetKey(KeyCode.W))
            flyingVehicleFlyingSystem.AddForwardInput(1.0f);
        else if (Input.GetKey(KeyCode.S))
            flyingVehicleFlyingSystem.AddForwardInput(-1.0f);

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            flyingVehicleFlyingSystem.StopMovingForward();

        // Hold down to turn left / right
        if (Input.GetKey(KeyCode.A))
            flyingVehicleFlyingSystem.AddYawInput(-1.0f);
        else if (Input.GetKey(KeyCode.D))
            flyingVehicleFlyingSystem.AddYawInput(1.0f);

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            flyingVehicleFlyingSystem.StopTurning();

        // Point up / down
        if (Input.GetKey(KeyCode.Q))
            flyingVehicleFlyingSystem.AddPitchInput(-1.0f);
        else if (Input.GetKey(KeyCode.E))
            flyingVehicleFlyingSystem.AddPitchInput(1.0f);

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
            flyingVehicleFlyingSystem.StopPitchInput();

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
                flyingVehicleFlyingSystem.AddForwardInput(joystick.inputAxisY);

                if (Mathf.Abs(joystick.inputAxisY) < 0.1)
                    flyingVehicleFlyingSystem.StopMovingForward();

                flyingVehicleFlyingSystem.AddYawInput(joystick.inputAxisX);
            }
        }
    }

    public void TakeOffOrLand()
    {
        if (flyingVehicleFlyingSystem.inAir)
        {
            flyingVehicleFlyingSystem.Land();

            rootRigidbody.useGravity = true;
            rootRigidbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            flyingVehicleFlyingSystem.TakeOff();

            rootRigidbody.useGravity = false;
            rootRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        takeOff = flyingVehicleFlyingSystem.inAir;
    }

    public void Boost()
    {
        flyingVehicleFlyingSystem.boosting = !flyingVehicleFlyingSystem.boosting;
        boosting = flyingVehicleFlyingSystem.boosting;
    }

    public void MobilePointUp()
    {
        flyingVehicleFlyingSystem.AddPitchInput(-1.0f);
    }

    public void MobilePointDown()
    {
        flyingVehicleFlyingSystem.AddPitchInput(1.0f);
    }

    public float GetFlyingSpeed()
    {
        return flyingVehicleFlyingSystem.flyingSpeed;
    }

    public float GetPowerPercentage()
    {
        return flyingVehicleFlyingSystem.powerPercentage;
    }

    public float GetWeightPercentage()
    {
        return flyingVehicleFlyingSystem.weightPercentage;
    }
}