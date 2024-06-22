using UnityEngine;
using UnityEngine.EventSystems;

public class Down : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            manager.MobileDown();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        manager.MobileReleaseDown();
    }
}