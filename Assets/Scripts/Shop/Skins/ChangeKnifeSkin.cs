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
        _materialIndex = PlayerPrefs.GetInt(ConstSystem.KNIFE_SKIN_INDEX);

        if (PlayerPrefs.HasKey(ConstSystem.KNIFE_SKIN_INDEX))
            _renderer.material = _materials[_materialIndex];
    }
}
