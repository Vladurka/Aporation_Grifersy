using UnityEngine;

public class PlaneFolower : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        transform.LookAt(_target.position);
    }
}
