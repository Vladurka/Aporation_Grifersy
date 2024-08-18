using UnityEngine;

public class ChangeSkinCar : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private MeshRenderer _renderer;

    private int _materialIndex;

    public void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
        _materialIndex = PlayerPrefsSafe.GetInt(ConstSystem.CAR_SKIN_INDEX);

        if (PlayerPrefsSafe.HasKey(ConstSystem.CAR_SKIN_INDEX))
            _renderer.material = _materials[_materialIndex];
    }
}
