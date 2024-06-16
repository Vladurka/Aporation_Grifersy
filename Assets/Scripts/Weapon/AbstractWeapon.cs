using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected float _callRange;
    [SerializeField] protected float _range;
    [SerializeField] protected float _damage;
    [SerializeField] protected Image _weapomImage;

    protected Camera _mainCamera;
    protected Camera _aimCamera;

    public int TotalBullets;
    public int Bullets;

    public abstract void Init();
    protected abstract IEnumerator Shoot(Camera cam);
}
