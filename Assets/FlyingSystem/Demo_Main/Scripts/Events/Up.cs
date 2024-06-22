using UnityEngine;
using UnityEngine.EventSystems;

public class Up : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            manager.MobileUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        manager.MobileReleaseUp();
    }
}