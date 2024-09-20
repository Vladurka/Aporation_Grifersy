using UnityEngine;

public class Gun : MonoBehaviour, IVehicleShoot
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private string _targetName = "Player";
    [SerializeField] private bool _findTarget = true;

    [HideInInspector] public GameObject Target;

    private void Start()
    {
        if(_findTarget)
            Target = GameObject.FindGameObjectWithTag(_targetName);
    }

    void Update()
    {
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - transform.position;
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
