using System.Collections;
using UnityEngine;

public class Raycasts : MonoBehaviour, IService
{
    [SerializeField] private GameObject _fixText;
    [SerializeField] private GameObject _missionText;

    [SerializeField] private float _range = 2f;
    private Camera _cam;

    public void Init()
    {
        _cam = Camera.main;
        if (_fixText != null && _missionText != null)
        {
            _fixText.SetActive(false);
            _missionText.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out AbstractTransport transport))
                {
                    if (ConstSystem.CanEnterHelicopter && hit.collider.CompareTag("Helicopter"))
                        transport.Enter();

                    if (!ConstSystem.CanEnterHelicopter && hit.collider.CompareTag("Helicopter"))
                        EnableText(_fixText);

                    if (hit.collider.CompareTag("Car"))
                        transport.Enter();
                }

                if (hit.collider.TryGetComponent(out IShop shop))
                    shop.Open();

                if (hit.collider.TryGetComponent(out IBox box))
                    box.Open();

                if (hit.collider.TryGetComponent(out IDoor door))
                    door.Open();

                if (hit.collider.TryGetComponent(out IInstrument instrument))
                    instrument.Get();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
            {
                if (hit.collider.TryGetComponent(out IStatesConntroller statesController))
                {
                    if (ConstSystem.CanFixHelicopter && hit.collider.CompareTag("Helicopter"))
                        statesController.SetFixedState();

                    else
                        EnableText(_missionText);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
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

    private void EnableText(GameObject text)
    {
        text.SetActive(true);
        StartCoroutine(DisableText(text));
    }

    private IEnumerator DisableText(GameObject text)
    {
        yield return new WaitForSeconds(5f);
        text.SetActive(false);
    }

    private void OnDisable()
    {
        if (_fixText != null && _missionText != null)
        {
            _fixText.SetActive(false);
            _missionText.SetActive(false);
        }
    }
}
