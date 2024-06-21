using UnityEngine;

public class Raycasts : MonoBehaviour, IService
{
    [SerializeField] private float _range = 2f;
    private Camera _cam;

    [Header("Helicopter")]
    public bool CanEnterHelicopter = false;
    public bool CanFixHelicopter = true;

    [Header("Car")]
    public bool CanEnterCar = false;
    public bool CanFixCar = true;

    public void Init()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out AbstractTransport transport))
                {
                    if (CanEnterHelicopter == true && hit.collider.CompareTag("Helicopter"))
                    {
                        transport.Enter();
                    }

                    if (CanEnterCar == true && hit.collider.CompareTag("Car"))
                    {
                        transport.Enter();
                    }
                }

                if(hit.collider.TryGetComponent(out IShop shop))
                    shop.Open();

                if(hit.collider.TryGetComponent(out IBox box))
                    box.Open();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out IStatesConntroller statesController))
                {
                    if (CanFixCar == true && hit.collider.CompareTag("Car"))
                        statesController.SetFixedState();

                    if (CanFixHelicopter == true && hit.collider.CompareTag("Helicopter"))
                        statesController.SetFixedState();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out AbstractTransport transport))
                {
                    transport.TransportReset();
                }
            }
        }
    }
}
