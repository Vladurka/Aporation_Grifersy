using UnityEngine;

public class ThirdPersonCharacter : MonoBehaviour
{
    private Transform characterTransform;
    private Rigidbody characterRigidbody;

    private Transform springArmTransform;
    public Transform characterCameraTransform;
    public Camera characterCamera;

    public Manager manager;

    public bool activated = true;

    public float cameraSpeed = 350.0f;
    public float characterMovementSpeed = 85.0f;

    private bool arrowKeyDown = false;

    void Start()
    {
        characterTransform = this.transform;
        characterRigidbody = this.GetComponent<Rigidbody>();

        // Detach spring arm from prefab
        springArmTransform = this.transform.GetChild(0).transform;
        springArmTransform.parent = null;
    }

    void Update()
    {
        if (activated && Manager.enabledControl)
        {
            CameraControlLogic();
            CharacterMovementLogic();

            if (Input.GetKeyUp(KeyCode.F))
                manager.UseFlyer();
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
        characterCamera.GetComponent<AudioListener>().enabled = false;
    }

    void CameraControlLogic()
    {
        springArmTransform.position = characterTransform.position;
        springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x + -Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
    }

    void CharacterMovementLogic()
    {
        arrowKeyDown = false;

        Vector3 lookAtPosition = characterTransform.position;

        if (Input.GetKey(KeyCode.W))
        {
            arrowKeyDown = true;
            lookAtPosition += new Vector3(springArmTransform.forward.x, 0.0f, springArmTransform.forward.z);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            arrowKeyDown = true;
            lookAtPosition -= new Vector3(springArmTransform.forward.x, 0.0f, springArmTransform.forward.z);
        }

        if (Input.GetKey(KeyCode.A))
        {
            arrowKeyDown = true;
            lookAtPosition -= new Vector3(springArmTransform.right.x, 0.0f, springArmTransform.right.z);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            arrowKeyDown = true;
            lookAtPosition += new Vector3(springArmTransform.right.x, 0.0f, springArmTransform.right.z);
        }

        if (arrowKeyDown)
        {
            characterTransform.LookAt(lookAtPosition);
            characterRigidbody.velocity = characterTransform.forward * characterMovementSpeed;
        }
        else
        {
            characterRigidbody.velocity = new Vector3(0.0f, characterRigidbody.velocity.y, 0.0f);
        }
    }
}