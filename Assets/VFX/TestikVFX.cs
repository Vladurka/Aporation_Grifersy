using UnityEngine;
using UnityEngine.VFX;

public class TestikVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect _effect;
    void Start()
    {
        _effect.Stop();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _effect.Play();
    }
}
