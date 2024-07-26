using UnityEngine;

public class CrushDetector : MonoBehaviour
{
<<<<<<< Updated upstream
    private PlaneController _controller;
=======
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _range  = 2f;
    [SerializeField] private string _tagToIgnore = "Missile";
    [SerializeField] private float _enableAngle = 20f;

    private EventBus _eventBus;

    private static bool _isDead  = false;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
                _controller.FlySpeed = 0;
                _controller.Rb.useGravity = true;   
                Debug.Log("Crush");
=======
                float angle = Vector3.Angle(transform.forward, hit.normal);

                if (hit.collider.CompareTag("Avianosec"))
                {
                    if (angle >= _enableAngle)
                        _eventBus.Invoke(new SetDie());
                }

                else
                    _eventBus.Invoke(new SetDie());
>>>>>>> Stashed changes
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }
}
