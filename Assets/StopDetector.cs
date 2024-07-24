using System.Collections;
using UnityEngine;

public class StopDetector : MonoBehaviour
{
    [SerializeField] private float _stopForce = 1f;

    private PlaneController _controller;

    private void Start()
    {
        _controller = FindAnyObjectByType<PlaneController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_controller.IsClosed && _controller.IsStarted && _controller.CanFly)
        {
            if(other.CompareTag("Avianosec"))
                StartCoroutine(Stoping());
        }
    }

    private IEnumerator Stoping()
    {
        if(_controller.FlySpeed > 0)
        {
            _controller.FlySpeed -= _stopForce;
            _controller.CanFly = false;
            _controller.IsStarted = false;
            _controller.IsSet = false;
            _controller.Rb.useGravity = true;
            _controller.Force = 0f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Stoping());
        }

        else
            yield return null;
    }
}
