using UnityEngine;

public class Raft : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("End");
            PlayerPrefs.SetInt("Prison", 1);
        }
    }
}
