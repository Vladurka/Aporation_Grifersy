using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyAsset
{
    public class MoveC130 : MonoBehaviour
    {
        public GameObject CameraC130;
        public GameObject CameraPlayer;
        public GameObject ButtonJump;
        public Transform Player;
        public Transform PlayerExit;
        public Transform Center;
        public int speedC130;
        public int speedVint;
        public Transform Vint1;
        public Transform Vint2;
        public Transform Vint3;
        public Transform Vint4;
        public void Start()
        {
            speedC130 = 5;
            speedVint = 2000;
            float x = Random.Range(50f, 1000f);
            float y = Random.Range(100f, 200f);
            float z = Random.Range(50f, 1000f);
            transform.position = new Vector3(x, y, z);
            transform.LookAt(Center);
        }
        Vector3 Forw = Vector3.forward;
        public void LateUpdate()
        {
            transform.Translate(Forw * speedC130 * Time.deltaTime);
            Vint1.transform.Rotate(0, speedVint * Time.deltaTime, 0); // Rotation paddle 1
            Vint2.transform.Rotate(0, speedVint * Time.deltaTime, 0); // Rotation paddle 2
            Vint3.transform.Rotate(0, speedVint * Time.deltaTime, 0); // Rotation paddle 3
            Vint4.transform.Rotate(0, speedVint * Time.deltaTime, 0); // Rotation paddle 4
        }

        public void Jump()
        {
            Player.transform.position = PlayerExit.transform.position;
            CameraC130.SetActive(false);
            CameraPlayer.SetActive(true);
            ButtonJump.SetActive(false);
        }
    }
}
