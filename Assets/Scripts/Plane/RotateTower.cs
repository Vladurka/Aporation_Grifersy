using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTower : MonoBehaviour
{
    private GameObject _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("F-14");
    }
    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = _target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}
