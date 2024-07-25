using System.Collections;
using UnityEngine;

public class StopDetector : MonoBehaviour, IService
{
    [SerializeField] private float _stopForce = 1.2f;

    private PlaneController _controller;

    public void Init()
    {
        _controller = ServiceLocator.Current.Get<PlaneController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_controller.IsClosed && _controller.IsStarted && _controller.CanFly)
        {
            if(other.CompareTag("Avianosec"))
                StartCoroutine(Stoping(_stopForce));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        _controller.CanFly = true;
        _controller.IsStarted = true;
        _controller.IsSet = true;
        _controller.Rb.useGravity = false;
        _controller.Force = 0.3f;
    }

    public IEnumerator Stoping(float stopForce)
    {
        if(_controller.FlySpeed > 0)
        {
            _controller.FlySpeed -= stopForce;
            _controller.CanFly = false;
            _controller.IsStarted = false;
            _controller.IsSet = false;
            _controller.Rb.useGravity = true;
            _controller.Force = 0f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Stoping(stopForce));
        }

        else
            yield return null;
    }
}
