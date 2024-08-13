using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;

public class WeaponCase : MonoBehaviour, IBox
{
    [SerializeField] private GameObject _weaponController;
    private Animator _animator;

    private EventBus _eventBus;

    private bool _isOpen = false;
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _animator = GetComponent<Animator>();
        _weaponController.SetActive(false);
    }

    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            _animator.SetTrigger("Open");
            _weaponController.SetActive(true);
            _eventBus.Invoke(new NextTip());
        }
    }
}
