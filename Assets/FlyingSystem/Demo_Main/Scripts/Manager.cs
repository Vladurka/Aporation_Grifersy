using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    /*
    0: Default
    1: Duck
    2: Eagle
    3: Dragon
    4: Airliner
    5: Jet
    6: Helicopter
    7: Drone
    8: Flying Car
    9: Humanoid Aircraft
    10: Parachute
    11: Glider
    12: Vite Aerea
    13: Paper Airplane
    */
    public int possessedControllerId = 0;

    [Header("Creatures")]
    public BirdController duckBirdController;
    public EagleController eagleController;
    public BirdController dragonBirdController;

    [Header("Controllers")]
    public ThirdPersonCharacter thirdPersonCharacterController;

    public AircraftController airlinerAircraftController;
    public AircraftController jetAircraftController;
    public HelicopterController helicopterController;
    public DroneController droneController;
    public FlyingCarController flyingCarController;
    public HumanoidAircraftController humanoidAircraftController;
    public GliderController parachuteGliderController;
    public GliderController gliderController;
    public ViteAereaController viteAereaController;
    public GliderController paperAirPlaneGliderController;

    public static bool enabledControl = false;

    private bool enabledCinemachine = false;
    public bool resetCameraLogic = false;

    private static bool mobileInputControl = false;

    [Header("UI")]
    public GameObject flyerLayer;
    public GameObject useImage;
    public GameObject pcImage, mobileImage, joystick, directionButtonGroup, rollButtonGroup;
    public GameObject downButton;
    public GameObject turnLeftButton, turnRightButton;

    public Button switchToPCOrMobileModeButton;

    public Text upButtonText, downButtonText;
    public Text takeOffOrLandText;

    public Text speedText;

    public Slider powerProgressBar, weightProgressBar;

    public Button takeOffOrLandButton;
    public GameObject takeOffImage, landImage;

    public Button boostButton;
    public GameObject boostImage, normalSpeedImage;

    public Button dropButton;

    public Button resetButton;

    public GameObject controlText1, controlText2, controlText3, controlText4, controlText5, controlText6, controlText7, controlText8, controlText9, controlText10, controlText11, controlText12, controlText13;

    void Start()
    {
        switchToPCOrMobileModeButton.onClick.AddListener(SwitchBetweenMobileAndPC);
        takeOffOrLandButton.onClick.AddListener(TakeOffOrLand);
        boostButton.onClick.AddListener(Boost);
        dropButton.onClick.AddListener(Drop);
        resetButton.onClick.AddListener(ResetCamera);

        flyerLayer.SetActive(false);
        useImage.SetActive(false);

        thirdPersonCharacterController.activated = true;
    }

    void Update()
    {
        if (!enabledControl && Input.GetMouseButtonUp(0))
        {
            LockMouse();

            Destroy(GameObject.Find("StartingCanvas"));
        }

        CinemachineLogic();

        if (Input.GetKeyUp(KeyCode.R))
            ResetCamera();

        ResetCameraLogic();

        if (Input.GetKeyUp(KeyCode.M))
            SwitchBetweenMobileAndPC();
        else if (Input.GetKeyUp(KeyCode.H))
            HideOrUnhideUI();


        bool result = false;

        if (possessedControllerId == 1)
            result = duckBirdController.takeOff;
        else if (possessedControllerId == 2)
            result = eagleController.takeOff;
        else if (possessedControllerId == 3)
            result = dragonBirdController.takeOff;
        else if (possessedControllerId == 4)
            result = airlinerAircraftController.takeOff;
        else if (possessedControllerId == 5)
            result = jetAircraftController.takeOff;
        else if (possessedControllerId == 6)
            result = !helicopterController.takeOff;
        else if (possessedControllerId == 7)
            result = !droneController.takeOff;
        else if (possessedControllerId == 8)
            result = !flyingCarController.takeOff;
        else if (possessedControllerId == 9)
            result = !humanoidAircraftController.hoverMode;

        takeOffImage.SetActive(!result);
        landImage.SetActive(result);

        if (possessedControllerId == 1)
            result = duckBirdController.boosting;
        else if (possessedControllerId == 2)
            result = eagleController.boosting;
        else if (possessedControllerId == 3)
            result = dragonBirdController.boosting;
        else if (possessedControllerId == 4)
            result = airlinerAircraftController.boosting;
        else if (possessedControllerId == 5)
            result = jetAircraftController.boosting;
        else if (possessedControllerId == 6)
            result = helicopterController.boosting;
        else if (possessedControllerId == 7)
            result = droneController.boosting;
        else if (possessedControllerId == 8)
            result = flyingCarController.boosting;
        else if (possessedControllerId == 9)
            result = humanoidAircraftController.boosting;

        boostImage.SetActive(result);
        normalSpeedImage.SetActive(!result);

        if (possessedControllerId == 1)
        {
            speedText.text = (int)duckBirdController.GetFlyingSpeed() + "";
            powerProgressBar.value = duckBirdController.GetStaminaPercentage();
            weightProgressBar.value = duckBirdController.GetWeightPercentage();
        }
        else if (possessedControllerId == 2)
        {
            speedText.text = (int)eagleController.GetFlyingSpeed() + "";
            powerProgressBar.value = eagleController.GetStaminaPercentage();
            weightProgressBar.value = eagleController.GetWeightPercentage();
        }
        else if (possessedControllerId == 3)
        {
            speedText.text = (int)dragonBirdController.GetFlyingSpeed() + "";
            powerProgressBar.value = dragonBirdController.GetStaminaPercentage();
            weightProgressBar.value = dragonBirdController.GetWeightPercentage();
        }
        else if (possessedControllerId == 4)
        {
            if (!airlinerAircraftController.takeOff)
                speedText.text = Mathf.Abs((int)airlinerAircraftController.currentGroundMovementSpeed) + "";
            else
                speedText.text = (int)airlinerAircraftController.GetFlyingSpeed() + "";

            powerProgressBar.value = airlinerAircraftController.GetPowerPercentage();
            weightProgressBar.value = airlinerAircraftController.GetWeightPercentage();
        }
        else if (possessedControllerId == 5)
        {
            if (!jetAircraftController.takeOff)
                speedText.text = Mathf.Abs((int)jetAircraftController.currentGroundMovementSpeed) + "";
            else
                speedText.text = (int)jetAircraftController.GetFlyingSpeed() + "";

            powerProgressBar.value = jetAircraftController.GetPowerPercentage();
            weightProgressBar.value = jetAircraftController.GetWeightPercentage();
        }
        else if (possessedControllerId == 6)
        {
            speedText.text = (int)helicopterController.GetFlyingSpeed() + "";
            powerProgressBar.value = helicopterController.GetPowerPercentage();
            weightProgressBar.value = helicopterController.GetWeightPercentage();
        }
        else if (possessedControllerId == 7)
        {
            speedText.text = (int)droneController.GetFlyingSpeed() + "";
            powerProgressBar.value = droneController.GetPowerPercentage();
            weightProgressBar.value = droneController.GetWeightPercentage();
        }
        else if (possessedControllerId == 8)
        {
            speedText.text = (int)flyingCarController.GetFlyingSpeed() + "";
            powerProgressBar.value = flyingCarController.GetPowerPercentage();
            weightProgressBar.value = flyingCarController.GetWeightPercentage();
        }
        else if (possessedControllerId == 9)
        {
            speedText.text = (int)humanoidAircraftController.GetFlyingSpeed() + "";
            powerProgressBar.value = humanoidAircraftController.GetPowerPercentage();
            weightProgressBar.value = humanoidAircraftController.GetWeightPercentage();
        }
        else if (possessedControllerId == 10)
        {
            speedText.text = (int)parachuteGliderController.GetFlyingSpeed() + "";
            weightProgressBar.value = parachuteGliderController.GetWeightPercentage();
        }
        else if (possessedControllerId == 11)
        {
            speedText.text = (int)gliderController.GetFlyingSpeed() + "";
            weightProgressBar.value = gliderController.GetWeightPercentage();
        }
        else if (possessedControllerId == 12)
        {
            speedText.text = (int)viteAereaController.GetFlyingSpeed() + "";
            weightProgressBar.value = viteAereaController.GetWeightPercentage();
        }
        else if (possessedControllerId == 13)
        {
            speedText.text = (int)paperAirPlaneGliderController.GetFlyingSpeed() + "";
            weightProgressBar.value = paperAirPlaneGliderController.GetWeightPercentage();
        }
    }

    public void UseFlyer()
    {
        if (thirdPersonCharacterController.activated && possessedControllerId != 0)
        {
            thirdPersonCharacterController.Deactivate();

            if (possessedControllerId == 4)
                airlinerAircraftController.transform.position = new Vector3(141.0f, 2.5f, 507.0f);
            else if (possessedControllerId == 5)
                jetAircraftController.transform.position = new Vector3(141.0f, 2.5f, 507.0f);
            else if (possessedControllerId == 9)
                humanoidAircraftController.transform.position = new Vector3(2.5f, 0.0f, 407.0f);
            else if (possessedControllerId == 10)
                parachuteGliderController.transform.position = new Vector3(100.0f, 150.0f, 460.0f);
            else if (possessedControllerId == 11)
                gliderController.transform.position = new Vector3(100.0f, 150.0f, 460.0f);
            else if (possessedControllerId == 12)
                viteAereaController.transform.position = new Vector3(100.0f, 150.0f, 460.0f);
            else if (possessedControllerId == 13)
                paperAirPlaneGliderController.transform.position = new Vector3(100.0f, 150.0f, 460.0f);

            InitializeUI();

            enabledCinemachine = true;
        }
    }

    public void CinemachineLogic()
    {
        if (enabledCinemachine)
        {
            if (possessedControllerId == 1)
            {
                if (CinemachineLerp(duckBirdController.characterCamera.transform.position, duckBirdController.characterCamera.transform.rotation))
                    duckBirdController.Activate();
            }
            else if (possessedControllerId == 2)
            {
                if (CinemachineLerp(eagleController.characterCamera.transform.position, eagleController.characterCamera.transform.rotation))
                    eagleController.Activate();
            }
            else if (possessedControllerId == 3)
            {
                if (CinemachineLerp(dragonBirdController.characterCamera.transform.position, dragonBirdController.characterCamera.transform.rotation))
                    dragonBirdController.Activate();
            }
            else if (possessedControllerId == 4)
            {
                if (CinemachineLerp(airlinerAircraftController.characterCamera.transform.position, airlinerAircraftController.characterCamera.transform.rotation))
                    airlinerAircraftController.Activate();
            }
            else if (possessedControllerId == 5)
            {
                if (CinemachineLerp(jetAircraftController.characterCamera.transform.position, jetAircraftController.characterCamera.transform.rotation))
                    jetAircraftController.Activate();
            }
            else if (possessedControllerId == 6)
            {
                if (CinemachineLerp(helicopterController.characterCamera.transform.position, helicopterController.characterCamera.transform.rotation))
                    helicopterController.Activate();
            }
            else if (possessedControllerId == 7)
            {
                if (CinemachineLerp(droneController.characterCamera.transform.position, droneController.characterCamera.transform.rotation))
                    droneController.Activate();
            }
            else if (possessedControllerId == 8)
            {
                if (CinemachineLerp(flyingCarController.characterCamera.transform.position, flyingCarController.characterCamera.transform.rotation))
                    flyingCarController.Activate();
            }
            else if (possessedControllerId == 9)
            {
                if (CinemachineLerp(humanoidAircraftController.characterCamera.transform.position, humanoidAircraftController.characterCamera.transform.rotation))
                    humanoidAircraftController.Activate();
            }
            else if (possessedControllerId == 10)
            {
                if (CinemachineLerp(parachuteGliderController.characterCamera.transform.position, parachuteGliderController.characterCamera.transform.rotation))
                {
                    parachuteGliderController.Activate();
                    parachuteGliderController.TakeOff(10.0f);
                }
            }
            else if (possessedControllerId == 11)
            {
                if (CinemachineLerp(gliderController.characterCamera.transform.position, gliderController.characterCamera.transform.rotation))
                {
                    gliderController.Activate();
                    gliderController.TakeOff(10.0f);
                }
            }
            else if (possessedControllerId == 12)
            {
                if (CinemachineLerp(viteAereaController.characterCamera.transform.position, viteAereaController.characterCamera.transform.rotation))
                {
                    viteAereaController.Activate();
                    viteAereaController.TakeOff(10.0f);
                }
            }
            else if (possessedControllerId == 13)
            {
                if (CinemachineLerp(paperAirPlaneGliderController.characterCamera.transform.position, paperAirPlaneGliderController.characterCamera.transform.rotation))
                {
                    paperAirPlaneGliderController.Activate();
                    paperAirPlaneGliderController.TakeOff(10.0f);
                }
            }
        }
    }

    bool CinemachineLerp(Vector3 targetPosition, Quaternion targetRotation)
    {
        thirdPersonCharacterController.characterCameraTransform.position = Vector3.Lerp(thirdPersonCharacterController.characterCameraTransform.position, targetPosition, 4.0f * Time.deltaTime);
        thirdPersonCharacterController.characterCameraTransform.rotation = Quaternion.Lerp(thirdPersonCharacterController.characterCameraTransform.rotation, targetRotation, 4.0f * Time.deltaTime);

        if (Vector3.Distance(thirdPersonCharacterController.characterCameraTransform.position, targetPosition) < 0.01f)
        {
            enabledCinemachine = false;

            thirdPersonCharacterController.characterCamera.enabled = false;
            thirdPersonCharacterController.characterCameraTransform.localPosition = Vector3.zero;
            thirdPersonCharacterController.characterCameraTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            return true;
        }

        return false;
    }

    void ResetCamera()
    {
        if (!resetCameraLogic)
        {
            mobileInputControl = false;
            LockMouse();

            flyerLayer.SetActive(false);

            if (possessedControllerId == 1)
            {
                thirdPersonCharacterController.characterCameraTransform.position = duckBirdController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = duckBirdController.characterCamera.transform.rotation;

                duckBirdController.Deactivate();
            }
            else if (possessedControllerId == 2)
            {
                thirdPersonCharacterController.characterCameraTransform.position = eagleController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = eagleController.characterCamera.transform.rotation;

                eagleController.Deactivate();
            }
            else if (possessedControllerId == 3)
            {
                thirdPersonCharacterController.characterCameraTransform.position = dragonBirdController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = dragonBirdController.characterCamera.transform.rotation;

                dragonBirdController.Deactivate();
            }
            else if (possessedControllerId == 4)
            {
                thirdPersonCharacterController.characterCameraTransform.position = airlinerAircraftController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = airlinerAircraftController.characterCamera.transform.rotation;

                airlinerAircraftController.Deactivate();
            }
            else if (possessedControllerId == 5)
            {
                thirdPersonCharacterController.characterCameraTransform.position = jetAircraftController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = jetAircraftController.characterCamera.transform.rotation;

                jetAircraftController.Deactivate();
            }
            else if (possessedControllerId == 6)
            {
                thirdPersonCharacterController.characterCameraTransform.position = helicopterController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = helicopterController.characterCamera.transform.rotation;

                helicopterController.Deactivate();
            }
            else if (possessedControllerId == 7)
            {
                thirdPersonCharacterController.characterCameraTransform.position = droneController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = droneController.characterCamera.transform.rotation;

                droneController.Deactivate();
            }
            else if (possessedControllerId == 8)
            {
                thirdPersonCharacterController.characterCameraTransform.position = flyingCarController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = flyingCarController.characterCamera.transform.rotation;

                flyingCarController.Deactivate();
            }
            else if (possessedControllerId == 9)
            {
                thirdPersonCharacterController.characterCameraTransform.position = humanoidAircraftController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = humanoidAircraftController.characterCamera.transform.rotation;

                humanoidAircraftController.Deactivate();
            }
            else if (possessedControllerId == 10)
            {
                thirdPersonCharacterController.characterCameraTransform.position = parachuteGliderController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = parachuteGliderController.characterCamera.transform.rotation;

                parachuteGliderController.Deactivate();
            }
            else if (possessedControllerId == 11)
            {
                thirdPersonCharacterController.characterCameraTransform.position = gliderController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = gliderController.characterCamera.transform.rotation;

                gliderController.Deactivate();
            }
            else if (possessedControllerId == 12)
            {
                thirdPersonCharacterController.characterCameraTransform.position = viteAereaController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = viteAereaController.characterCamera.transform.rotation;

                viteAereaController.Deactivate();
            }
            else if (possessedControllerId == 13)
            {
                thirdPersonCharacterController.characterCameraTransform.position = paperAirPlaneGliderController.characterCamera.transform.position;
                thirdPersonCharacterController.characterCameraTransform.rotation = paperAirPlaneGliderController.characterCamera.transform.rotation;

                paperAirPlaneGliderController.Deactivate();
            }

            thirdPersonCharacterController.Activate();

            resetCameraLogic = true;
        }
    }

    void ResetCameraLogic()
    {
        if (resetCameraLogic)
        {
            if (ResetCameraLerp())
            {
                resetCameraLogic = false;

                possessedControllerId = 0;
            }
        }
    }

    bool ResetCameraLerp()
    {
        thirdPersonCharacterController.characterCameraTransform.localPosition = Vector3.Lerp(thirdPersonCharacterController.characterCameraTransform.localPosition, new Vector3(0.0f, 3.0f, -10.0f), 4.0f * Time.deltaTime);
        thirdPersonCharacterController.characterCameraTransform.localRotation = Quaternion.Lerp(thirdPersonCharacterController.characterCameraTransform.localRotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), 4.0f * Time.deltaTime);

        if (Vector3.Distance(thirdPersonCharacterController.characterCameraTransform.localPosition, new Vector3(0.0f, 3.0f, -10.0f)) < 0.01f)
        {
            resetCameraLogic = false;

            thirdPersonCharacterController.characterCameraTransform.localPosition = new Vector3(0.0f, 3.0f, -10.0f);
            thirdPersonCharacterController.characterCameraTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            return true;
        }

        return false;
    }

    public void SetUseImageVisibility(bool enabled)
    {
        useImage.SetActive(enabled);
    }

    public void SetControlTextVisibility(bool enabled)
    {
        if (possessedControllerId == 1)
            controlText1.SetActive(enabled);
        else if (possessedControllerId == 2)
            controlText2.SetActive(enabled);
        else if (possessedControllerId == 3)
            controlText3.SetActive(enabled);
        else if (possessedControllerId == 4)
            controlText4.SetActive(enabled);
        else if (possessedControllerId == 5)
            controlText5.SetActive(enabled);
        else if (possessedControllerId == 6)
            controlText6.SetActive(enabled);
        else if (possessedControllerId == 7)
            controlText7.SetActive(enabled);
        else if (possessedControllerId == 8)
            controlText8.SetActive(enabled);
        else if (possessedControllerId == 9)
            controlText9.SetActive(enabled);
        else if (possessedControllerId == 10)
            controlText10.SetActive(enabled);
        else if (possessedControllerId == 11)
            controlText11.SetActive(enabled);
        else if (possessedControllerId == 12)
            controlText12.SetActive(enabled);
        else if (possessedControllerId == 13)
            controlText13.SetActive(enabled);
    }

    void InitializeUI()
    {
        useImage.SetActive(false);
        flyerLayer.SetActive(true);

        pcImage.SetActive(mobileInputControl);
        mobileImage.SetActive(!mobileInputControl);
        joystick.SetActive(mobileInputControl && possessedControllerId < 10);

        directionButtonGroup.SetActive(possessedControllerId > 3);

        downButton.SetActive(possessedControllerId != 9);

        turnLeftButton.SetActive(possessedControllerId != 8 && possessedControllerId != 9);
        turnRightButton.SetActive(possessedControllerId != 8 && possessedControllerId != 9);

        rollButtonGroup.SetActive(possessedControllerId != 6 && possessedControllerId != 7 && possessedControllerId != 8 && possessedControllerId != 9);

        takeOffOrLandButton.gameObject.SetActive(possessedControllerId < 10);
        boostButton.gameObject.SetActive(possessedControllerId < 10);

        powerProgressBar.gameObject.SetActive(possessedControllerId < 10);

        dropButton.gameObject.SetActive(possessedControllerId == 2);

        if (possessedControllerId == 6)
        {
            upButtonText.text = "[W]";
            downButtonText.text = "[S]";
        }
        else if (possessedControllerId == 9)
            upButtonText.text = "[Space]";
        else
        {
            upButtonText.text = "[Q]";
            downButtonText.text = "[E]";
        }

        if (possessedControllerId == 9)
            takeOffOrLandText.text = "[C]";
        else
            takeOffOrLandText.text = "[Space]";
    }

    void SwitchBetweenMobileAndPC()
    {
        mobileInputControl = !mobileInputControl;

        InitializeUI();

        if (mobileInputControl)
            UnlockMouse();
        else
            LockMouse();

        if (possessedControllerId == 1)
            duckBirdController.mobileInputControl = !duckBirdController.mobileInputControl;
        else if (possessedControllerId == 2)
            eagleController.mobileInputControl = !eagleController.mobileInputControl;
        else if (possessedControllerId == 3)
            dragonBirdController.mobileInputControl = !dragonBirdController.mobileInputControl;
        else if (possessedControllerId == 4)
            airlinerAircraftController.mobileInputControl = !airlinerAircraftController.mobileInputControl;
        else if (possessedControllerId == 5)
            jetAircraftController.mobileInputControl = !jetAircraftController.mobileInputControl;
        else if (possessedControllerId == 6)
            helicopterController.mobileInputControl = !helicopterController.mobileInputControl;
        else if (possessedControllerId == 7)
            droneController.mobileInputControl = !droneController.mobileInputControl;
        else if (possessedControllerId == 8)
            flyingCarController.mobileInputControl = !flyingCarController.mobileInputControl;
        else if (possessedControllerId == 9)
            humanoidAircraftController.mobileInputControl = !humanoidAircraftController.mobileInputControl;
        else if (possessedControllerId == 10)
            parachuteGliderController.mobileInputControl = !parachuteGliderController.mobileInputControl;
        else if (possessedControllerId == 11)
            gliderController.mobileInputControl = !gliderController.mobileInputControl;
        else if (possessedControllerId == 12)
            viteAereaController.mobileInputControl = !viteAereaController.mobileInputControl;
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.mobileInputControl = !paperAirPlaneGliderController.mobileInputControl;
    }

    void LockMouse()
    {
        enabledControl = true;

        // Lock mouse cursor in the view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockMouse()
    {
        enabledControl = true;

        // Lock mouse cursor in the view
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideOrUnhideUI()
    {
        flyerLayer.SetActive(!flyerLayer.activeSelf);
    }

    public void TakeOffOrLand()
    {
        if (possessedControllerId == 1)
            duckBirdController.TakeOff();
        else if (possessedControllerId == 2)
            eagleController.TakeOff();
        else if (possessedControllerId == 3)
            dragonBirdController.TakeOff();
        else if (possessedControllerId == 4)
            airlinerAircraftController.TakeOffOrLand();
        else if (possessedControllerId == 5)
            jetAircraftController.TakeOffOrLand();
        else if (possessedControllerId == 6)
            helicopterController.TakeOffOrLand();
        else if (possessedControllerId == 7)
            droneController.TakeOffOrLand();
        else if (possessedControllerId == 8)
            flyingCarController.TakeOffOrLand();
        else if (possessedControllerId == 9)
            humanoidAircraftController.SwitchHoverModeOnOrOff();
    }

    public void Boost()
    {
        if (possessedControllerId == 1)
            duckBirdController.Boost();
        else if (possessedControllerId == 2)
            eagleController.Boost();
        else if (possessedControllerId == 3)
            dragonBirdController.Boost();
        else if (possessedControllerId == 4)
            airlinerAircraftController.Boost();
        else if (possessedControllerId == 5)
            jetAircraftController.Boost();
        else if (possessedControllerId == 6)
            helicopterController.Boost();
        else if (possessedControllerId == 7)
            droneController.Boost();
        else if (possessedControllerId == 8)
            flyingCarController.Boost();
        else if (possessedControllerId == 9)
            humanoidAircraftController.Boost();
    }

    public void Drop()
    {
        if (possessedControllerId == 2)
            eagleController.Drop();
    }

    public void MobileTurnLeft()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobileTurnLeft();
        else if (possessedControllerId == 5)
            jetAircraftController.MobileTurnLeft();
        else if (possessedControllerId == 6)
            helicopterController.MobileTurnLeft();
        else if (possessedControllerId == 7)
            droneController.MobileTurnLeft();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobileTurnLeft();
        else if (possessedControllerId == 11)
            gliderController.MobileTurnLeft();
        else if (possessedControllerId == 12)
            viteAereaController.MobileTurnLeft();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileTurnLeft();
    }

    public void MobileTurnRight()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobileTurnRight();
        else if (possessedControllerId == 5)
            jetAircraftController.MobileTurnRight();
        else if (possessedControllerId == 6)
            helicopterController.MobileTurnRight();
        else if (possessedControllerId == 7)
            droneController.MobileTurnRight();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobileTurnRight();
        else if (possessedControllerId == 11)
            gliderController.MobileTurnRight();
        else if (possessedControllerId == 12)
            viteAereaController.MobileTurnRight();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileTurnRight();
    }

    public void MobileReleaseTurnLeft()
    {
        if (possessedControllerId == 10)
            parachuteGliderController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 11)
            gliderController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 12)
            viteAereaController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileReleaseTurnLeft();
    }

    public void MobileReleaseTurnRight()
    {
        if (possessedControllerId == 10)
            parachuteGliderController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 11)
            gliderController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 12)
            viteAereaController.MobileReleaseTurnLeft();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileReleaseTurnLeft();
    }

    public void MobileUp()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobilePointUp();
        else if (possessedControllerId == 5)
            jetAircraftController.MobilePointUp();
        else if (possessedControllerId == 6)
            helicopterController.Ascend();
        else if (possessedControllerId == 7)
            droneController.Ascend();
        else if (possessedControllerId == 8)
            flyingCarController.MobilePointUp();
        else if (possessedControllerId == 9)
            humanoidAircraftController.VerticalBoost();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobilePointUp();
        else if (possessedControllerId == 11)
            gliderController.MobilePointUp();
        else if (possessedControllerId == 12)
            viteAereaController.MobilePointUp();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobilePointUp();
    }

    public void MobileDown()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobilePointDown();
        else if (possessedControllerId == 5)
            jetAircraftController.MobilePointDown();
        else if (possessedControllerId == 6)
            helicopterController.Descend();
        else if (possessedControllerId == 7)
            droneController.Descend();
        else if (possessedControllerId == 8)
            flyingCarController.MobilePointDown();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobilePointDown();
        else if (possessedControllerId == 11)
            gliderController.MobilePointDown();
        else if (possessedControllerId == 12)
            viteAereaController.MobilePointDown();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobilePointDown();
    }

    public void MobileReleaseUp()
    {
        if (possessedControllerId == 6)
            helicopterController.MobileStopAscendOrDescend();
        else if (possessedControllerId == 7)
            droneController.MobileStopAscendOrDescend();
    }

    public void MobileReleaseDown()
    {
        if (possessedControllerId == 6)
            helicopterController.MobileStopAscendOrDescend();
        else if (possessedControllerId == 7)
            droneController.MobileStopAscendOrDescend();
    }

    public void MobileRollLeft()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobileRollLeft();
        else if (possessedControllerId == 5)
            jetAircraftController.MobileRollLeft();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobileRollLeft();
        else if (possessedControllerId == 11)
            gliderController.MobileRollLeft();
        else if (possessedControllerId == 12)
            viteAereaController.MobileRollLeft();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileRollLeft();
    }

    public void MobileRollRight()
    {
        if (possessedControllerId == 4)
            airlinerAircraftController.MobileRollRight();
        else if (possessedControllerId == 5)
            jetAircraftController.MobileRollRight();
        else if (possessedControllerId == 10)
            parachuteGliderController.MobileRollRight();
        else if (possessedControllerId == 11)
            gliderController.MobileRollRight();
        else if (possessedControllerId == 12)
            viteAereaController.MobileRollRight();
        else if (possessedControllerId == 13)
            paperAirPlaneGliderController.MobileRollRight();
    }
}