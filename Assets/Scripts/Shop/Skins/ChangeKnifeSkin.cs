using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKnifeSkin : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private MeshRenderer _renderer;

    private int _materialIndex;

    public void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
        _materialIndex = PlayerPrefsSafe.GetInt(ConstSystem.KNIFE_SKIN_INDEX);

        if (PlayerPrefsSafe.HasKey(ConstSystem.KNIFE_SKIN_INDEX))
            _renderer.material = _materials[_materialIndex];
    }
}
