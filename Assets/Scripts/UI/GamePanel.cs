using UnityEngine.UI;
using UnityEngine;
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;

public class GamePanel : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private Text _currentBulletsText;
    [SerializeField] private Text _totalBulletsText;

    [Header("Health")]
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _syrgineText;
    [SerializeField] private Slider _healthBar;

    [Header("Money")]
    [SerializeField] private Text _moneyText;

    [Header("Icons")]
    [SerializeField] private Image[] _images;

    [Header("Drone")]
    [SerializeField] private GameObject _dronePanel;
    [SerializeField] private Text _droneAmountText;

    [Header("Car")]
    [SerializeField] private GameObject _speedometr;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetCurrentBullets>(SetCurrentBulletsText, 1);
        _eventBus.Subscribe<SetTotalBullets>(SetTotalBulletsText, 1);
        _eventBus.Subscribe<UpdateCurrentBullets>(UpdateCurrentBulletsText, 1);
        _eventBus.Subscribe<UpdateTotalBullets>(UpdateTotalBulletsText, 1);
        _eventBus.Subscribe<UpdateHealth>(UpdateHealthText, 1);
        _eventBus.Subscribe<UpdateMoney>(UpdateMoneyText, 1);
        _eventBus.Subscribe<UpdateSyrgine>(UpdateSyrgineText, 1);
        _eventBus.Subscribe<SetImage>(SetImage, 1);
        _eventBus.Subscribe<SetDronePanel>(SetDronePanel, 1);
        _eventBus.Subscribe<UpdateDrone>(UpdateDroneText, 1);
        _eventBus.Subscribe<SetSpeedometer>(SetSpeedometer, 1);

        foreach (Image image in _images)
            image.enabled = false;
    }

    private void UpdateCurrentBulletsText(UpdateCurrentBullets currentBullets)
    {
        _currentBulletsText.text = currentBullets.Amount.ToString() + " /";
    }

    private void UpdateTotalBulletsText(UpdateTotalBullets totalBullets)
    {
        _totalBulletsText.text = totalBullets.Amount.ToString();
    }

    private void UpdateHealthText(UpdateHealth health)
    {
        _healthText.text = health.Amount.ToString();
        _healthBar.value = health.Amount;
    }

    private void UpdateMoneyText(UpdateMoney updateMoney)
    {
        _moneyText.text = updateMoney.Amount.ToString();
    }

    private void SetCurrentBulletsText(SetCurrentBullets state)
    {
        _currentBulletsText.enabled = state.State;
    }

    private void SetTotalBulletsText(SetTotalBullets state)
    {
        _totalBulletsText.enabled = state.State;
    }

    private void UpdateSyrgineText(UpdateSyrgine updateSyrgine)
    {
        _syrgineText.text = updateSyrgine.Amount.ToString();
    }

    private void SetImage(SetImage images)
    {
        foreach (Image image in _images)
            image.enabled = false;

        if(images.Active)
            _images[images.Index].enabled = true;
    }

    private void UpdateDroneText(UpdateDrone drone)
    {
        _droneAmountText.text = drone.Amount.ToString();
    }

    private void SetDronePanel(SetDronePanel drone)
    {
        if (_dronePanel != null)
            _dronePanel.SetActive(drone.State);
    }

    private void SetSpeedometer(SetSpeedometer speedometer)
    {
        if (_speedometr != null)
            _speedometr.SetActive(speedometer.State);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetCurrentBullets>(SetCurrentBulletsText);
        _eventBus.Unsubscribe<SetTotalBullets>(SetTotalBulletsText);
        _eventBus.Unsubscribe<UpdateCurrentBullets>(UpdateCurrentBulletsText);
        _eventBus.Unsubscribe<UpdateTotalBullets>(UpdateTotalBulletsText);
        _eventBus.Unsubscribe<UpdateHealth>(UpdateHealthText);
        _eventBus.Unsubscribe<UpdateMoney>(UpdateMoneyText);
        _eventBus.Unsubscribe<UpdateSyrgine>(UpdateSyrgineText);
        _eventBus.Unsubscribe<SetImage>(SetImage);
        _eventBus.Unsubscribe<SetDronePanel>(SetDronePanel);
        _eventBus.Unsubscribe<UpdateDrone>(UpdateDroneText);
        _eventBus.Unsubscribe<SetSpeedometer>(SetSpeedometer);
    }
}
