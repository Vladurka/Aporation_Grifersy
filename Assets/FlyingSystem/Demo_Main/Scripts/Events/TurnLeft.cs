using UnityEngine;
using UnityEngine.EventSystems;

public class TurnLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Manager manager;

    private bool buttonDown = false;

    void Start()
    {
        manager = GameObject.Find("Scripts").GetComponent<Manager>();
    }

    void Update()
    {
        if (buttonDown)
            manager.MobileTurnLeft();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        manager.MobileReleaseTurnLeft();
    }
}