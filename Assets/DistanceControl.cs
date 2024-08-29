using UnityEngine;

public class DistanceControl : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 20000f;
    void Update()
    {
        if (Vector3.Distance(transform.position, _target.position) >= _distance)
            Debug.Log(Vector3.Distance(transform.position, _target.position));
    }
}
