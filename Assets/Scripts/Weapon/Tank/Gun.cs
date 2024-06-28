using Game.SeniorEventBus.Signals;
using Game.SeniorEventBus;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    private GameObject _mainCharacter;

    private EventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<DestroyTank>(SetFalse, 2);
        _mainCharacter = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (_mainCharacter != null)
        {
            Vector3 direction = _mainCharacter.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        }
    }

    private void SetFalse(DestroyTank tank)
    {
        this.enabled = false;   
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<DestroyTank>(SetFalse);
    }
}
