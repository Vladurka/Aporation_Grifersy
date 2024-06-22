using UnityEngine;

using FlyingSystem;

public class GliderController : MonoBehaviour
{
    public Transform springArmTransform;
    public Camera characterCamera;

    public Rigidbody rootRigidbody;

    private GliderFlyingSystem gliderFlyingSystem;

    private Airflow airflow;

    public bool activated = false;

    public bool autoTakeOff = false;

    public float autoTakeOffSpeed = 10.0f;

    public float cameraSpeed = 300.0f;

    private float targetSpringArmRotationX, targetSpringArmRotationY;

    [Header("Mobile")]
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    void Start()
    {
        gliderFlyingSystem = this.GetComponent<GliderFlyingSystem>();

        if (autoTakeOff)
            gliderFlyingSystem.TakeOff(autoTakeOffSpeed);

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
            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * mobileCameraSpeed * Time.deltaTime;
            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * mobileCameraSpeed * Time.deltaTime;
        }
        else
        {
            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x;
            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y;
        }

        // Only detects on mobile devices
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

        springArmTransform.rotation = Quaternion.Euler(targetSpringArmRotationX, targetSpringArmRotationY, 0.0f);
    }

    void PCInputControlLogic()
    {
        // Hold down to turn left / right
        if (Input.GetKey(KeyCode.A))
            gliderFlyingSystem.AddYawInput(-1.0f);
        else if (Input.GetKey(KeyCode.D))
            gliderFlyingSystem.AddYawInput(1.0f);

        if (Input.GetKeyUp(KeyCode.A))
            gliderFlyingSystem.StopYawInput();
        else if (Input.GetKeyUp(KeyCode.D))
            gliderFlyingSystem.StopYawInput();

        // Point up / down
        if (Input.GetKey(KeyCode.Q))
            gliderFlyingSystem.AddPitchInput(-1.0f);
        else if (Input.GetKey(KeyCode.E))
            gliderFlyingSystem.AddPitchInput(1.0f);

        // Roll to have a sharp turn left / right
        if (Input.GetKey(KeyCode.Z))
            gliderFlyingSystem.AddRollInput(-1.0f);
        else if (Input.GetKey(KeyCode.C))
            gliderFlyingSystem.AddRollInput(1.0f);
    }

    public void MobileTurnLeft()
    {
        gliderFlyingSystem.AddYawInput(-1.0f);
    }

    public void MobileTurnRight()
    {
        gliderFlyingSystem.AddYawInput(1.0f);
    }

    public void MobileReleaseTurnLeft()
    {
        gliderFlyingSystem.StopYawInput();
    }

    public void MobileReleaseTurnRight()
    {
        gliderFlyingSystem.StopYawInput();
    }

    public void MobilePointUp()
    {
        gliderFlyingSystem.AddPitchInput(-1.0f);
    }

    public void MobilePointDown()
    {
        gliderFlyingSystem.AddPitchInput(1.0f);
    }

    public void MobileRollLeft()
    {
        gliderFlyingSystem.AddRollInput(-1.0f);
    }

    public void MobileRollRight()
    {
        gliderFlyingSystem.AddRollInput(1.0f);
    }

    public void TakeOff(float takeOffSpeed)
    {
        gliderFlyingSystem.TakeOff(takeOffSpeed);
    }

    public float GetFlyingSpeed()
    {
        return gliderFlyingSystem.flyingSpeed;
    }

    public float GetWeightPercentage()
    {
        return gliderFlyingSystem.weightPercentage;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Road")
        {
            if (gliderFlyingSystem.inAir)
                gliderFlyingSystem.Land();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Airflow")
        {
            airflow = other.GetComponent<Airflow>();
            gliderFlyingSystem.AddAirflowForce(airflow.intensity, airflow.acceleration, airflow.fadeOutAcceleration);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Airflow")
            gliderFlyingSystem.EndAirflowForce();
    }
}