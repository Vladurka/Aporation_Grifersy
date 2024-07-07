using UnityEngine;
using UnityEngine.UI;

public class ButtonShape : MonoBehaviour
{

    [SerializeField] private float _alpha = 0.1f;
    
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = _alpha;
    }

}