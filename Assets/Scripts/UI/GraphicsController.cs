using System.Collections.Generic;
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
    private RefreshRate _refreshRate;
    private float _targetFPS;


    private void Start()
    {
        #region Quality
        if (PlayerPrefsSafe.HasKey(_qualityKey))
            QualitySettings.SetQualityLevel(PlayerPrefsSafe.GetInt(_qualityKey), true);
        else
            QualitySettings.SetQualityLevel(5, true);

        if (PlayerPrefsSafe.HasKey(_qualityDropdownKey))
            _qualityDropdown.value = PlayerPrefsSafe.GetInt(_qualityDropdownKey);
        else
            _qualityDropdown.value = 2;

        #endregion

        #region Resolution

        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            _resolutionDropdown.options.Add(new Dropdown.OptionData(option));

            if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        if (PlayerPrefsSafe.HasKey(_resolutionKey))
            currentResolutionIndex = PlayerPrefsSafe.GetInt(_resolutionKey);

        ChangeResolution(currentResolutionIndex);

        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        #endregion

        #region FPS

        _refreshRate = Screen.currentResolution.refreshRateRatio;
        _targetFPS = _refreshRate.numerator / _refreshRate.denominator + 1;

        if (PlayerPrefsSafe.HasKey(_fpsKey))
            SetFPS(PlayerPrefsSafe.GetInt(_fpsKey));
        else
            SetFPS(_targetFPS);

        if (PlayerPrefsSafe.HasKey(_fpsDropdownKey))
            _fpsDropdown.value = PlayerPrefsSafe.GetInt(_fpsDropdownKey);
        else
            _fpsDropdown.value = 7;

        #endregion

        FullScreen();
    }

    #region  Toogle
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

    public void ScreenModeChanged(Toggle change)
    {
        if (change.isOn)
            WindowMode();

        else
            FullScreen();
    }

    #endregion

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
        PlayerPrefsSafe.SetInt(_qualityKey, amount);
    }

    public void DropdownQualityChanged(Dropdown change)
    {
        PlayerPrefsSafe.SetInt(_qualityDropdownKey, change.value);
    }
    #endregion

    #region Resolution
    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefsSafe.SetInt(_resolutionKey, resolutionIndex);
    }
    #endregion

    #region FPS
    public void DropDownFPS(int value)
    {
        if (value == 0)
            SetFPS(-1f);

        if (value == 1)
            SetFPS(360f);

        if (value == 2)
            SetFPS(240);

        if (value == 3)
            SetFPS(144f);

        if (value == 4)
            SetFPS(120f);

        if (value == 5)
            SetFPS(60f);

        if (value == 6)
            SetFPS(30f);

        if (value == 7)
            SetFPS(_targetFPS);
    }

    public void SetFPS(float amount)
    {
        Application.targetFrameRate = (int)amount;
        PlayerPrefsSafe.SetFloat(_fpsKey, amount);
    }

    public void DropdownFPSChanged(Dropdown change)
    {
        PlayerPrefsSafe.SetInt(_fpsDropdownKey, change.value);
    }
    #endregion
}