using UnityEngine;
<<<<<<< HEAD
using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
=======
>>>>>>> parent of a7405a9b (dwad)

public class CrushDetector : MonoBehaviour
{
    private PlaneController _controller;
<<<<<<< HEAD
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _range  = 2f;
    [SerializeField] private string _tagToIgnore = "Missile";
    [SerializeField] private float _enableAngle = 20f;

    private EventBus _eventBus;

    private static bool _isDead  = false;
=======
>>>>>>> parent of a7405a9b (dwad)

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
<<<<<<< HEAD
                float angle = Vector3.Angle(transform.forward, hit.normal);

                if (hit.collider.CompareTag("Avianosec"))
                {
                    if (angle >= _enableAngle)
                        _eventBus.Invoke(new SetDie());
                }

                else
                    _eventBus.Invoke(new SetDie());
=======
>>>>>>> parent of a7405a9b (dwad)
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }
}
