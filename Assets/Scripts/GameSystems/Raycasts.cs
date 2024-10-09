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
            HandleInteraction();

        if (Input.GetKeyDown(KeyCode.Q))
            HandleFixing();

        if (Input.GetKeyDown(KeyCode.C))
            HandleTransportReset();
    }

    private bool TryPerformRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range);
    }

    private void HandleInteraction()
    {
        if (TryPerformRaycast(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out AbstractTransport transport))
            {
                if (hit.collider.CompareTag("Helicopter"))
                {
                    if (ConstSystem.CanEnterHelicopter)
                        transport.Enter();
                    else
                        EnableText(_fixText);
                }

                else if (hit.collider.CompareTag("Car"))
                    transport.Enter();
            }

            if (hit.collider.TryGetComponent(out IOpenClose openClose)) openClose.Open();
            if (hit.collider.TryGetComponent(out IBox box)) box.Open();
            if (hit.collider.TryGetComponent(out IInstrument instrument)) instrument.Get();
            if (hit.collider.TryGetComponent(out IHostage hostage)) hostage.Save();
        }
    }

    private void HandleFixing()
    {
        if (TryPerformRaycast(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out IStatesConntroller statesController))
            {
                if (ConstSystem.CanFixHelicopter)
                    statesController.SetFixedState();

                else
                    EnableText(_missionText);
            }
        }
    }

    private void HandleTransportReset()
    {
        if (TryPerformRaycast(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out AbstractTransport transport))
                transport.TransportReset();
        }
    }

    private void EnableText(GameObject text)
    {
        _fixText.SetActive(false);
        _missionText.SetActive(false);  

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
