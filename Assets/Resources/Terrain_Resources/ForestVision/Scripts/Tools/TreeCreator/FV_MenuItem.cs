using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
//============================================================================
//
//			Used for ForestVision Menu Items
//
//============================================================================
namespace ForestVision.FV_TreeEditor
{
    public static class MenuItems
    {

        #region Main Menu Tools
        [MenuItem("Tools/ForestVision/FV All Tools", false, 30)]//access to everything
        private static void ShowTools() { FV_Tools.ShowEditor(); }

        [MenuItem("Tools/ForestVision/Tools/Switch Mesh", false, 30)]//access to everything
        private static void ShowToolSwitchMesh() { FV_SwitchMesh.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/Swap Material", false, 30)]//access to everything
        private static void ShowToolSwapMaterial() { FV_SwapMat.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/New Optimized", false, 30)]//access to everything
        private static void ShowToolNewOptimized() { FV_Collapse.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/New Prefab", false, 30)]//access to everything
        private static void ShowToolNewPrefab() { FV_Prefabber.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/Renamer", false, 50)]//access to everything
        private static void ShowToolRenamer() { FV_Renamer.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/Tweaker", false, 50)]//access to everything
        private static void ShowToolTweaker() { FV_Tweaker.ShowWindow(); }

        [MenuItem("Tools/ForestVision/Tools/Screenshot", false, 50)]//access to everything
        private static void ShowToolScreenshot() { FV_Screenshots.ShowWindow(); }



        [MenuItem("Tools/ForestVision/Deprecated/Browser", false, 30)]//access to everything
        private static void ShowEditor()
        {
            FV_Browser.ShowEditor();
        }


        [MenuItem("Tools/ForestVision/Deprecated/Tree Tools", false, 30)]//access to everything
        private static void ShowToolTreeTools()
        {
            FV_TreeTools.ShowWindow();
        }

        #endregion

        #region Assets

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Trunk", false, 30)]
        private static void CreateStraightTrunkv1()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("_trunk_01_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Spruce", false, 30)]
        private static void CreateBranchFoliageSpruce()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("SpruceBranch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Basic", false, 30)]
        private static void CreateBranchFoliageBasic1()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_basic_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }
        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Droopy", false, 30)]
        private static void CreateBranchFoliageDroopy()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_droopy_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;

        }
        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Piney", false, 30)]
        private static void CreateBranchFoliagePiney()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_pine_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }
        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Thin", false, 30)]
        private static void CreateBranchFoliageThin()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_thin_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }
        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Veiny", false, 30)]
        private static void CreateBranchFoliageVeiny()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_veiny_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }
        [MenuItem("GameObject/3D Object/ForestVision/Assets/Branches/Wavy", false, 30)]
        private static void CreateBranchFoliageWavy()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("foliage_wavy_Branch_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }



        [MenuItem("GameObject/3D Object/ForestVision/Assets/Leaves", false, 30)]
        private static void CreateLeavesBasic()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("_Leaves_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }


        [MenuItem("GameObject/3D Object/ForestVision/Assets/Card Leaves", false, 30)]
        private static void CreateLeavesCard1()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("_Card_1_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Hi Plant", false, 50)]
        private static void CreateHiPlant()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("HP_01", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Grass", false, 50)]
        private static void CreateGrass()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("Grasses_01", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Flower", false, 50)]
        private static void CreateFlowers()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("Flowers_01", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Rock", false, 50)]
        private static void CreateRocks()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("Rocks_01_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Scatter", false, 50)]
        private static void CreateScatter()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("Scatter_01_v1", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        [MenuItem("GameObject/3D Object/ForestVision/Assets/Woods Background", false, 70)]
        private static void CreateBG()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("ThickWoods", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }
        #endregion

    }

}
#endif