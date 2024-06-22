using UnityEngine;

using FlyingSystem;

public class BirdController : MonoBehaviour
{
    private Transform characterTransform;

    public Transform springArmTransform;
    public Camera characterCamera;

    public Animator animator;

    private CreatureFlyingSystem creatureFlyingSystem;

    private AudioSource audioSource;

    private Airflow airflow;

    public bool activated = false;

    [Header("General Attributes")]
    public bool takeOff;
    public bool boosting;

    public float cameraSpeed = 300.0f;

    [Range(0.0f, 100.0f)]
    public float springArmSmoothingFactor = 0.25f;

    private float targetSpringArmRotationX, targetSpringArmRotationY;

    public float groundMovementSpeed = 1.0f;

    [Header("Mobile")]
    public Joystick joystick;
    public bool mobileInputControl = false;
    public float mobileCameraSpeed = 300.0f;
    private float screenCenterX;

    void Start()
    {
        characterTransform = this.transform;

        creatureFlyingSystem = this.GetComponent<CreatureFlyingSystem>();

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
                PCInputControlLogic();
                CameraControlLogic();
            }
            else
            {
                MobileInputControlLogic();
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

    void PCInputControlLogic()
    {
        // Take off / grab
        if (Input.GetKeyUp(KeyCode.Space))
            TakeOff();

        // Fly forward / stop
        if (Input.GetKey(KeyCode.W))
            creatureFlyingSystem.FlyForward();
        else if (Input.GetKey(KeyCode.S))
            creatureFlyingSystem.SlowDown();
        else if (Input.GetKeyUp(KeyCode.S))
            creatureFlyingSystem.StopSlowingDown();

        // Turn left / right
        creatureFlyingSystem.AddYawInput(Input.GetAxis("Mouse X"));

        AnimationLogic();

        // Boost on / off
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            Boost();
    }

    void MobileInputControlLogic()
    {
        if (joystick != null)
        {
            if (joystick.inputAxisY > 0.01f)
                creatureFlyingSystem.FlyForward();
            else if (joystick.inputAxisY < -0.85f)
                creatureFlyingSystem.SlowDown();
            else if (creatureFlyingSystem.slowingDown && joystick.inputAxisY > -0.85f)
                creatureFlyingSystem.StopSlowingDown();

            AnimationLogic();
        }
    }

    void AnimationLogic()
    {
        if (creatureFlyingSystem.canDive)
        {
            if (creatureFlyingSystem.diving)
            {
                animator.SetBool("FlyToGlide", true);
                animator.SetBool("GlideToFly", false);
            }
            else
            {
                animator.SetBool("GlideToFly", true);
                animator.SetBool("FlyToGlide", false);
            }
        }
    }

    void CameraControlLogic()
    {
        springArmTransform.position = Vector3.Lerp(characterTransform.position, springArmTransform.position, springArmSmoothingFactor * Time.deltaTime);
        springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
    }

    void MobileCameraControlLogic()
    {
        // Temporarily use mouse to simulate the touch
        if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
        {
            targetSpringArmRotationX = springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * mobileCameraSpeed * Time.deltaTime;
            targetSpringArmRotationY = springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * mobileCameraSpeed * Time.deltaTime;

            creatureFlyingSystem.AddYawInput(Input.GetAxis("Mouse X") * mobileCameraSpeed * Time.deltaTime);
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

        //            creatureFlyingSystem.AddYawInput(Input.GetTouch(i).deltaPosition.x * mobileCameraSpeed * Time.deltaTime);
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

    public void TakeOff()
    {
        if (!creatureFlyingSystem.inAir)
        {
            creatureFlyingSystem.TakeOff();
            takeOff = creatureFlyingSystem.inAir;

            animator.SetBool("FlyToIdle", false);
            animator.SetBool("IdleToFly", true);

            animator.SetBool("GlideToIdle", false);

            audioSource.Play();
        }
    }

    public void Boost()
    {
        creatureFlyingSystem.boosting = !creatureFlyingSystem.boosting;
        boosting = creatureFlyingSystem.boosting;
    }

    public float GetFlyingSpeed()
    {
        return creatureFlyingSystem.flyingSpeed;
    }

    public float GetStaminaPercentage()
    {
        return creatureFlyingSystem.staminaPercentage;
    }

    public float GetWeightPercentage()
    {
        return creatureFlyingSystem.weightPercentage;
    }

    void OnCollisionEnter(Collision collision)
    {
        // The target collision can be anything like ground, terrain, etc.
        if (collision.collider.name == "Road")
        {
            if (creatureFlyingSystem.inAir)
            {
                creatureFlyingSystem.Land();
                takeOff = creatureFlyingSystem.inAir;

                animator.SetBool("GlideToIdle", true);

                animator.SetBool("FlyToIdle", true);
                animator.SetBool("IdleToFly", false);

                animator.SetBool("FlyToGlide", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Entering the airflow can lift up the flyer
        if (other.name == "Airflow")
        {
            airflow = other.GetComponent<Airflow>();

            creatureFlyingSystem.AddAirflowForce(airflow.intensity, airflow.acceleration, airflow.fadeOutAcceleration);
            creatureFlyingSystem.stopFlying = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Airflow")
        {
            creatureFlyingSystem.EndAirflowForce();
            creatureFlyingSystem.stopFlying = false;
        }
    }
}