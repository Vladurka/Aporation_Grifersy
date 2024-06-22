using UnityEngine;

using FlyingSystem;

public class AircraftController : MonoBehaviour
{
    public Rigidbody aircraftRigidbody;
    public Transform rootTransform;
    public Transform springArmTransform;
    public Camera characterCamera;

    public Transform rollRootTransform;

    private AircraftFlyingSystem aircraftFlyingSystem;

    private AudioSource audioSource;

    public bool activated = false;

    public float cameraSpeed = 300.0f;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;
    public float maximumGroundMovementSpeed = 80.0f;
    public float groundAcceleration = 20.0f;
    public float groundTurningSpeed = 10.0f;
    public float slowDownAccelerationAfterLanding = 10.0f;

    private bool landed = true;

    [HideInInspector]
    public float currentGroundMovementSpeed;

    private float targetSpringArmRotationX, targetSpringArmRotationY;

    private bool movingForward = false;

    private Vector3 targetVelocity;

    [Header("Mobile")]
    public Joystick joystick;
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    void Start()
    {
        aircraftFlyingSystem = this.GetComponent<AircraftFlyingSystem>();

        audioSource = this.GetComponent<AudioSource>();

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

            MovementLogic();
        }
    }

    public void Activate()
    {
        activated = true;
        characterCamera.enabled = true;
        characterCamera.GetComponent<AudioListener>().enabled = true;

        aircraftFlyingSystem.enabledFlyingLogic = true;

        audioSource.Play();
    }

    public void Deactivate()
    {
        activated = false;
        characterCamera.enabled = false;
        characterCamera.GetComponent<AudioListener>().enabled = false;

        aircraftFlyingSystem.enabledFlyingLogic = false;

        audioSource.Stop();
    }

    void PCCameraControlLogic()
    {
        targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
        targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;

        // Camera view follows the roll
        springArmTransform.rotation = Quaternion.Euler(targetSpringArmRotationX, targetSpringArmRotationY, rollRootTransform.rotation.eulerAngles.z);
    }

    void MobileCameraControlLogic()
    {
        // Temporarily use mouse to simulate the touch in Unity editor
        if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
        {
            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * mobileCameraSpeed * Time.deltaTime;
            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * mobileCameraSpeed * Time.deltaTime;
        }
        else
        {
            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x;
            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y;
        }

        // Only for mobile devices(uncomment the following and test on physical mobile devices)
        //if (Input.touchCount > 0)
        //{
        //    for (var i = 0; i < Input.touchCount; i++)
        //    {
        //        if (Input.GetTouch(i).position.x > screenCenterX && Input.GetTouch(i).phase == TouchPhase.Moved)
        //        {
        //            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetTouch(i).deltaPosition.y * mobileCameraSpeed * Time.deltaTime;
        //            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetTouch(i).deltaPosition.x * mobileCameraSpeed * Time.deltaTime;
        //        }
        //    }
        //}
        //else
        //{
        //    targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x;
        //    targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y;
        //}

        springArmTransform.rotation = Quaternion.Euler(targetSpringArmRotationX, targetSpringArmRotationY, rollRootTransform.rotation.eulerAngles.z);
    }

    void PCInputControlLogic()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            TakeOffOrLand();

        if (!aircraftFlyingSystem.inAir)
        {
            if (Input.GetKey(KeyCode.W))
                GoundMoveForward(1.0f);
            else if (Input.GetKey(KeyCode.S))
                GoundMoveForward(-1.0f);

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                StopMovingForward();

            if (Input.GetKey(KeyCode.A))
                GoundTurnRight(-1.0f);
            else if (Input.GetKey(KeyCode.D))
                GoundTurnRight(1.0f);
        }
        else
        {
            // Hold down to turn left / right
            if (Input.GetKey(KeyCode.A))
                aircraftFlyingSystem.AddYawInput(-1.0f);
            else if (Input.GetKey(KeyCode.D))
                aircraftFlyingSystem.AddYawInput(1.0f);

            if (Input.GetKeyUp(KeyCode.A))
                aircraftFlyingSystem.StopYawInput();
            else if (Input.GetKeyUp(KeyCode.D))
                aircraftFlyingSystem.StopYawInput();

            // Point up / down
            if (Input.GetKey(KeyCode.Q))
                aircraftFlyingSystem.AddPitchInput(1.0f);
            else if (Input.GetKey(KeyCode.E))
                aircraftFlyingSystem.AddPitchInput(-1.0f);

            // Roll to have a sharp turn left / right
            if (Input.GetKey(KeyCode.Z))
                aircraftFlyingSystem.AddRollInput(-1.0f);
            else if (Input.GetKey(KeyCode.C))
                aircraftFlyingSystem.AddRollInput(1.0f);

            // Boost on / off
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                Boost();
        }
    }

    void MobileInputControlLogic()
    {
        if (joystick != null)
        {
            if (!aircraftFlyingSystem.inAir)
            {
                if (Mathf.Abs(joystick.inputAxisY) > 0.01f)
                    GoundMoveForward(joystick.inputAxisY);
                else
                    StopMovingForward();

                if (Mathf.Abs(joystick.inputAxisX) > 0.01f)
                    GoundTurnRight(joystick.inputAxisX);
            }
        }
    }

    public void GoundMoveForward(float value)
    {
        movingForward = true;

        if (!(currentGroundMovementSpeed > maximumGroundMovementSpeed))
        {
            if (value > 0.0f)
                currentGroundMovementSpeed = Mathf.Clamp(currentGroundMovementSpeed + groundAcceleration * Time.deltaTime, -maximumGroundMovementSpeed, maximumGroundMovementSpeed);
            else
                currentGroundMovementSpeed = Mathf.Clamp(currentGroundMovementSpeed - groundAcceleration * Time.deltaTime, -maximumGroundMovementSpeed, maximumGroundMovementSpeed);

            targetVelocity = rootTransform.forward * Mathf.Abs(value) * currentGroundMovementSpeed;
            aircraftRigidbody.velocity = new Vector3(targetVelocity.x, aircraftRigidbody.velocity.y, targetVelocity.z);
        }
    }

    public void StopMovingForward()
    {
        movingForward = false;
    }

    public void GoundTurnRight(float value)
    {
        rootTransform.Rotate(rootTransform.up * value * groundTurningSpeed * Time.deltaTime);
    }

    public void TakeOffOrLand()
    {
        if (!aircraftFlyingSystem.inAir)
        {
            // Only the speed is higher than minimum allowed speed can take off
            if (aircraftFlyingSystem.TakeOff(currentGroundMovementSpeed))
            {
                // Rigidbody is not used when in air
                aircraftRigidbody.useGravity = false;

                landed = false;
            }
        }
        else
        {
            aircraftFlyingSystem.Land();

            // Enable gravity after landing
            aircraftRigidbody.useGravity = true;

            movingForward = false;
            landed = true;
        }

        takeOff = aircraftFlyingSystem.inAir;
    }

    public void Boost()
    {
        aircraftFlyingSystem.boosting = !aircraftFlyingSystem.boosting;
        boosting = aircraftFlyingSystem.boosting;
    }

    void MovementLogic()
    {
        if (landed)
        {
            if (!movingForward)
            {
                if (currentGroundMovementSpeed > 0.0f)
                    currentGroundMovementSpeed = Mathf.Clamp(currentGroundMovementSpeed - slowDownAccelerationAfterLanding * Time.deltaTime, 0.0f, currentGroundMovementSpeed);
                else
                    currentGroundMovementSpeed = Mathf.Clamp(currentGroundMovementSpeed + slowDownAccelerationAfterLanding * Time.deltaTime, currentGroundMovementSpeed, 0.0f);

                targetVelocity = rootTransform.forward * currentGroundMovementSpeed;
                aircraftRigidbody.velocity = new Vector3(targetVelocity.x, aircraftRigidbody.velocity.y, targetVelocity.z);
            }
            else
            {
                // It is possible that the speed is higher than maximum after landing, but will gradually decrease because of ground friction
                if (currentGroundMovementSpeed > maximumGroundMovementSpeed)
                {
                    currentGroundMovementSpeed -= slowDownAccelerationAfterLanding * Time.deltaTime;
                    
                    targetVelocity = rootTransform.forward * currentGroundMovementSpeed;
                    aircraftRigidbody.velocity = new Vector3(targetVelocity.x, aircraftRigidbody.velocity.y, targetVelocity.z);
                }
            }
        }
    }

    public void MobileTurnLeft()
    {
        aircraftFlyingSystem.AddYawInput(-1.0f);
    }

    public void MobileTurnRight()
    {
        aircraftFlyingSystem.AddYawInput(1.0f);
    }

    public void MobilePointUp()
    {
        aircraftFlyingSystem.AddPitchInput(1.0f);
    }

    public void MobilePointDown()
    {
        aircraftFlyingSystem.AddPitchInput(-1.0f);
    }

    public void MobileRollLeft()
    {
        aircraftFlyingSystem.AddRollInput(-1.0f);
    }

    public void MobileRollRight()
    {
        aircraftFlyingSystem.AddRollInput(1.0f);
    }

    public float GetFlyingSpeed()
    {
        return aircraftFlyingSystem.flyingSpeed;
    }

    public float GetPowerPercentage()
    {
        return aircraftFlyingSystem.powerPercentage;
    }

    public float GetWeightPercentage()
    {
        return aircraftFlyingSystem.weightPercentage;
    }
}