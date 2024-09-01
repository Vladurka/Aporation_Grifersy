using DG.Tweening;
using UnityEngine;

public class Buckhweat : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new Vector3(0f, 360f, 0f), 10f);

        //transform.DOMoveY(transform.position.y + 0.1f, 0.5f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.InOutSine);
    }
}
