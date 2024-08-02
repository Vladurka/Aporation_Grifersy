using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPreview : MonoBehaviour
{
    public void Preview(Material material)
    {
        if (transform.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = material;
        }

        if (transform.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
        {
            if (!transform.CompareTag("Gloves"))
                skinnedMeshRenderer.material = material;

            if (transform.CompareTag("Gloves"))
            {
                Material[] Materials = skinnedMeshRenderer.materials;
                Materials[1] = material;
                skinnedMeshRenderer.materials = Materials;
            }

        }
    }
}
