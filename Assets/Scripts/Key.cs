using UnityEngine;

public class Key : MonoBehaviour
{
    private bool _isKey = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 3f))
            {
                if(hit.collider.CompareTag("Key"))
                {
                    _isKey = true;
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.TryGetComponent(out IDoor door) && _isKey)
                    door.Open();
            }
        }
    }
}
