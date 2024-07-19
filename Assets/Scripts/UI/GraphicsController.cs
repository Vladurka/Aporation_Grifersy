using UnityEngine;
using UnityEngine.UI;

public class GraphicsController : MonoBehaviour
{
    [Header("Quality")]

    [SerializeField] private Dropdown _qualityDropdown;
    private string _qualityKey = "Quality";
    private string _qualityDropdownKey = "QualityDropdown";

    [Header("Resolution")]

    [SerializeField] private Dropdown _resolutionDropdown;
    private Resolution[] _resolutions;
    private string _resolutionKey = "Resolution";

    [Header("FPS")]
    [SerializeField] private Dropdown _fpsDropdown;
    private string _fpsKey = "FPS";
    private string _fpsDropdownKey = "FPSdropdown";



    private void Start()
    {
        #region Quality
        if (PlayerPrefs.HasKey(_qualityKey))
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(_qualityKey), true);

        if (!PlayerPrefs.HasKey(_qualityKey))
            QualitySettings.SetQualityLevel(5, true);

        if (_qualityDropdown != null)
        {
            if (PlayerPrefs.HasKey(_qualityDropdownKey))
                _qualityDropdown.value = PlayerPrefs.GetInt(_qualityDropdownKey);

            if (!PlayerPrefs.HasKey(_qualityDropdownKey))
                _qualityDropdown.value = 2;
        }

        #endregion

        #region Resolution

        _resolutions = Screen.resolutions;

        if (_resolutionDropdown != null)
            _resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutionDropdown != null)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                _resolutionDropdown.options.Add(new Dropdown.OptionData(option));
            }

            if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
            {
                    currentResolutionIndex = i;
            }
        }

        if (PlayerPrefs.HasKey(_resolutionKey))
            currentResolutionIndex = PlayerPrefs.GetInt(_resolutionKey);

        ChangeResolution(currentResolutionIndex);
        Debug.Log(Screen.resolutions[currentResolutionIndex]);

        if (_resolutionDropdown != null)
        {
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        #endregion

        #region FPS

        if (PlayerPrefs.HasKey(_fpsKey))
            SetFPS(PlayerPrefs.GetInt(_fpsKey));

        if (!PlayerPrefs.HasKey(_fpsKey))
            SetFPS(144);

        if (_fpsDropdown != null)
        {
            if (PlayerPrefs.HasKey(_fpsDropdownKey))
                _fpsDropdown.value = PlayerPrefs.GetInt(_fpsDropdownKey);

            if (!PlayerPrefs.HasKey(_fpsDropdownKey))
                _fpsDropdown.value = 3;
        }

        #endregion
    }

    public void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;
    }

    public void WindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
    }

    #region Quality
    public void DropDownQuality(int value)
    {
        if (value == 0)
        {
            SetQuality(0);
        }

        if (value == 1)
        {
            SetQuality(2);
        }

        if (value == 2)
            SetQuality(5);
    }

    public void SetQuality(int amount)
    {
        QualitySettings.SetQualityLevel(amount, true);
        PlayerPrefs.SetInt(_qualityKey, amount);
    }

    public void DropdownQualityChanged(Dropdown change)
    {
        PlayerPrefs.SetInt(_qualityDropdownKey, change.value);
    }
    #endregion

    #region Resolution
    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt(_resolutionKey, resolutionIndex);
    }
    #endregion

    #region FPS
    public void DropDownFPS(int value)
    {
        if (value == 0)
            SetFPS(500);

        if (value == 1)
            SetFPS(360);

        if (value == 2)
            SetFPS(240);

        if (value == 3)
            SetFPS(144);

        if (value == 4)
            SetFPS(120);

        if (value == 5)
            SetFPS(60);

        if (value == 6)
            SetFPS(45);

        if (value == 7)
            SetFPS(30);
    }

    public void SetFPS(int amount)
    {
        Application.targetFrameRate = amount;
        PlayerPrefs.SetInt(_fpsKey, amount);
        Debug.Log(amount);
    }

    public void DropdownFPSChanged(Dropdown change)
    {
        PlayerPrefs.SetInt(_fpsDropdownKey, change.value);
    }
    #endregion
}

