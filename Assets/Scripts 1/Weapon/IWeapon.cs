using System.Collections;
using UnityEngine;

public interface IWeapon
{
    public int TotalBullets { get; set; }
    public float Damage { get; set; }
    public float CallRange { get; set; }

    public IEnumerator Shoot(Camera cam);

}
