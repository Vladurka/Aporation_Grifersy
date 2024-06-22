using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    public Manager manager;

    public int flyerId;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "ThirdPersonCharacter")
        {
            manager.SetUseImageVisibility(true);
            manager.possessedControllerId = flyerId;
            manager.SetControlTextVisibility(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "ThirdPersonCharacter")
        {
            manager.SetUseImageVisibility(false);
            manager.SetControlTextVisibility(false);
            manager.possessedControllerId = 0;
        }
    }
}