using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Transform joystickBackgroundTransform, joystickControllerTransform;

    private bool reset = false;

    private float maxMovingDistance;

    public bool isMoving = false;

    public float x, y;
    public float inputAxisX, inputAxisY;

    void Start()
    {
        maxMovingDistance = 77.5f;
    }

    void Update()
    {
        if (reset)
        {
            joystickControllerTransform.position = new Vector3(joystickControllerTransform.position.x - (joystickControllerTransform.position.x - joystickBackgroundTransform.position.x) * 10.0f * Time.deltaTime, joystickControllerTransform.position.y - (joystickControllerTransform.position.y - joystickBackgroundTransform.position.y) * 10.0f * Time.deltaTime, 0.0f);

            x = (joystickControllerTransform.position.x - joystickBackgroundTransform.position.x);
            y = joystickControllerTransform.position.y - joystickBackgroundTransform.position.y;

            inputAxisX = x / maxMovingDistance;
            inputAxisY = y / maxMovingDistance;

            if (joystickControllerTransform.position.x < (joystickBackgroundTransform.position.x + 1.0f) && (joystickBackgroundTransform.position.x - 1f) < joystickControllerTransform.position.x)
            {
                joystickControllerTransform.position = new Vector3(joystickBackgroundTransform.position.x, joystickBackgroundTransform.position.y, 0f);
                reset = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isMoving == false)
            isMoving = true;

        if (isMoving)
        {
            float realtimeDistance = (eventData.position.x - joystickBackgroundTransform.position.x) * (eventData.position.x - joystickBackgroundTransform.position.x) + (eventData.position.y - joystickBackgroundTransform.position.y) * (eventData.position.y - joystickBackgroundTransform.position.y);

            if (realtimeDistance <= maxMovingDistance * maxMovingDistance)
            {
                joystickControllerTransform.position = new Vector3(eventData.position.x, eventData.position.y, 0.0f);
            }
            else
            {
                float relativeX = Mathf.Sqrt((maxMovingDistance * maxMovingDistance) / (1.0f + ((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y)) * ((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y))));

                if (eventData.position.x > joystickBackgroundTransform.position.x)
                {
                    if (eventData.position.y > joystickBackgroundTransform.position.y)
                        joystickControllerTransform.position = new Vector3(joystickBackgroundTransform.position.x + relativeX * Mathf.Abs((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y + 0.000001f)), joystickBackgroundTransform.position.y + relativeX, 0.0f);
                    else
                        joystickControllerTransform.position = new Vector3(joystickBackgroundTransform.position.x + relativeX * Mathf.Abs((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y + 0.000001f)), joystickBackgroundTransform.position.y - relativeX, 0.0f);
                }
                else
                {
                    if (eventData.position.y > joystickBackgroundTransform.position.y)
                        joystickControllerTransform.position = new Vector3(joystickBackgroundTransform.position.x - relativeX * Mathf.Abs((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y + 0.000001f)), joystickBackgroundTransform.position.y + relativeX, 0.0f);
                    else
                        joystickControllerTransform.position = new Vector3(joystickBackgroundTransform.position.x - relativeX * Mathf.Abs((eventData.position.x - joystickBackgroundTransform.position.x) / (eventData.position.y - joystickBackgroundTransform.position.y + 0.000001f)), joystickBackgroundTransform.position.y - relativeX, 0.0f);
                }
            }

            x = (joystickControllerTransform.position.x - joystickBackgroundTransform.position.x);
            y = joystickControllerTransform.position.y - joystickBackgroundTransform.position.y;

            inputAxisX = x / maxMovingDistance;
            inputAxisY = y / maxMovingDistance;

            reset = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMoving = false;
        reset = true;
    }
}