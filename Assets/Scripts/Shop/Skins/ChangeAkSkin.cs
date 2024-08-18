using UnityEngine;

public class ChangeAkSkin : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private int _materialIndex;

    public void Init()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _materialIndex = PlayerPrefsSafe.GetInt(ConstSystem.AK_SKIN_INDEX);

        if (PlayerPrefsSafe.HasKey(ConstSystem.AK_SKIN_INDEX))
            _skinnedMeshRenderer.material = _materials[_materialIndex];
    }
}
