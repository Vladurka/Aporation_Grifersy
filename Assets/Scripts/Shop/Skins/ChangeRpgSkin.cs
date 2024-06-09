using UnityEngine;

public class ChangeRpgSkin : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private int _materialIndex;
    private SkinnedMeshRenderer _renderer;

    public void Init()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
        _materialIndex = PlayerPrefs.GetInt(ConstSystem.RPG_SKIN_INDEX);

        if(PlayerPrefs.HasKey(ConstSystem.RPG_SKIN_INDEX))
            _renderer.material = _materials[_materialIndex];
    }
}
