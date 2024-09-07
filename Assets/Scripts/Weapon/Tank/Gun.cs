using UnityEngine;

public class Gun : MonoBehaviour, IVehicleShoot
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private string _targetName = "Player";

    private GameObject _mainCharacter;

    private void Start()
    {
        _mainCharacter = GameObject.FindGameObjectWithTag(_targetName);
    }

    void Update()
    {
        if (_mainCharacter != null)
        {
            Vector3 direction = _mainCharacter.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        }
    }

    public void Stop()
    {
        this.enabled = false;   
    }
}
