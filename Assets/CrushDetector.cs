using UnityEngine;

public class CrushDetector : MonoBehaviour
{
    private PlaneController _controller;

    private void Start()
    {
        _controller = FindAnyObjectByType<PlaneController>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider)
            {
                _controller.FlySpeed = 0;
                _controller.Rb.useGravity = true;   
                Debug.Log("Crush");
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }
}
