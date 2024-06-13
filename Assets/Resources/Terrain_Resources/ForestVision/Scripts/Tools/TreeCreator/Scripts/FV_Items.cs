using UnityEngine;

namespace ForestVision.FV_TreeEditor
{
    public class FV_Items : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum Category
        {
            Backgrounds, Branches, Flora, Ground, LowPoly, Mobile, Optimized, Specials, TreesYoung, Trees, TreesOld, Trunks, Vines
        }

        public Category category = Category.Trunks;
        public string itemName = "";

#endif
    }
}


