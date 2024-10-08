using UnityEngine;
using System.Collections;

public class CheckForTip : MonoBehaviour
{
    [SerializeField] private GameObject _panelIntercat;
    [SerializeField] private GameObject _panelFix;
    [SerializeField] private GameObject _panelPlant;

    private Camera _cam;

    [SerializeField] private bool _isMission2 = false;

    private void Start()
    {
        _cam = Camera.main;
        StartCoroutine(Check());
    }

    private void OnEnable()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        yield return new WaitForSeconds(0.5f);

        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, 2f))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) || hit.collider.CompareTag("Key"))
                _panelIntercat.SetActive(true);

            else if (hit.collider.TryGetComponent(out AbstractTransport car) && hit.collider.CompareTag("Car"))
                _panelIntercat.SetActive(true);

            else if (hit.collider.TryGetComponent(out AbstractTransport helicopter) && hit.collider.CompareTag("Helicopter"))
            {
                if (ConstSystem.CanEnterHelicopter)
                    _panelIntercat.SetActive(true);

                if (!ConstSystem.CanEnterHelicopter)
                    _panelFix.SetActive(true);
            }

            else if (_isMission2 && hit.collider.CompareTag("Tank"))
                _panelPlant.SetActive(true);


            else
            {
                if (_panelIntercat != null)
                    _panelIntercat.SetActive(false);

                if (_panelFix != null)
                    _panelFix.SetActive(false);

                if (_panelPlant != null)
                    _panelPlant.SetActive(false);
            }
        }

        else
        {
            if (_panelIntercat != null)
                _panelIntercat.SetActive(false);

            if (_panelFix != null)
                _panelFix.SetActive(false);

            if (_panelPlant != null)
                _panelPlant.SetActive(false);
        }

        StartCoroutine(Check());
    }

    private void OnDisable()
    {
        if (_panelIntercat != null)
            _panelIntercat.SetActive(false);

        if (_panelFix != null)
            _panelFix.SetActive(false);

        if (_panelPlant != null)
            _panelPlant.SetActive(false);

        StopAllCoroutines();
    }
}
