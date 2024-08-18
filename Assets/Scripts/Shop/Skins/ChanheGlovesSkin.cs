using UnityEngine;

public class ChanheGlovesSkin : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    [SerializeField] private int _materialIndex;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _materialIndex = PlayerPrefsSafe.GetInt(ConstSystem.GLOVES_SKIN_INDEX);

        if (PlayerPrefsSafe.HasKey(ConstSystem.GLOVES_SKIN_INDEX))
        {
            Material[] Materials = _skinnedMeshRenderer.materials;
            Materials[1] = _materials[_materialIndex];
            _skinnedMeshRenderer.materials = Materials;
        }
    }
}
