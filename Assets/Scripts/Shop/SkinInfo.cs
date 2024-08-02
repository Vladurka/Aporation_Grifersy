using UnityEngine;

public class SkinInfo : MonoBehaviour
{
    [SerializeField] private SkinData _skinData;

    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _useButton;
    public void Start()
    {
        _skinData.BuyButton = _buyButton;
        _skinData.UseButton = _useButton;

        _skinData.SaveCondition = PlayerPrefs.GetInt(_skinData.Key);

        if (_skinData.SaveCondition == 1)
            CanUse();

        if (!PlayerPrefs.HasKey(_skinData.Key) || _skinData.SaveCondition == 0)
            CantUse();
    }

    public void CanUse()
    {
        _skinData.UseButton.SetActive(true);
        _skinData.BuyButton.SetActive(false);
        PlayerPrefs.SetInt(_skinData.Key, 1);
        PlayerPrefs.Save();
    }

    private void CantUse()
    {
        _skinData.BuyButton.SetActive(true);
        _skinData.UseButton.SetActive(false);
        PlayerPrefs.SetInt(_skinData.Key, 0);
        PlayerPrefs.Save();
    }

    public void SkinPreview(Material material)
    {
        if (transform.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = material;
        }

        if (transform.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
        {
            if(!transform.CompareTag("Gloves"))
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