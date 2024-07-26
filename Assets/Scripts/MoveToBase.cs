using UnityEngine;

public class MoveToBase : MonoBehaviour
{
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _car;
    [SerializeField] private GameObject _helicopter;

    public void Move()
    {
        Vector3 character = new Vector3(1332.5f, 10.8f, 680.4f);
        Vector3 car = new Vector3(1337.42f, 11f, 671.2f);
        Vector3 helicopter = new Vector3(1308.45f, 10f, 666.13f);

        _mainCharacter.transform.localPosition = character;
        _car.transform.localPosition = car;

        if(ConstSystem.CanEnterHelicopter)
            _helicopter.transform.localPosition = helicopter;
    }
}
