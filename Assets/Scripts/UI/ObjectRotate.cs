using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 200f;

    [SerializeField] private bool _rotateX = true;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);

            if(_rotateX)
                transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
