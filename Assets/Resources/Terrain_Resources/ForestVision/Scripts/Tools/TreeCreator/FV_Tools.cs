using UnityEngine;
using ForestVision.FV_TreeEditor;

#if UNITY_EDITOR
using UnityEditor;

public class FV_Tools : EditorWindow
{

    public static FV_Tools instance;
    private int guiSpace = 20;

    public static void ShowEditor()
    {
        instance = (FV_Tools)EditorWindow.GetWindow(typeof(FV_Tools));
        instance.titleContent = new GUIContent("ForestVision Tools");
        instance.autoRepaintOnSceneChange = true;
        instance.titleContent = new GUIContent("FV Tools");
    }

    private void OnGUI()
    {

        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Switch Mesh", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {

            FV_SwitchMesh.ShowWindow();

        }
        GUILayout.Space(guiSpace);

        if (GUILayout.Button("Swap Material", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {

            FV_SwapMat.ShowWindow();

        }
        GUILayout.Space(guiSpace);


        // ----------------- Collapse Mesh ----------------------

        if (GUILayout.Button("New Optimized", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            FV_Collapse.ShowWindow();
        }

        GUILayout.Space(guiSpace);
        //-------------------- Save Prefab -----------------------


        if (GUILayout.Button("New Prefab", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            FV_Prefabber.ShowWindow();
        }

        GUILayout.Space(guiSpace);

        //-------------------- Rename -----------------------
        if (GUILayout.Button("Renamer", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            FV_Renamer.ShowWindow();

        }
        GUILayout.Space(guiSpace);

        if (GUILayout.Button("+ FV_BranchRotation", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            var go = Selection.activeGameObject;
            if (go == null)
            {
                if (EditorUtility.DisplayDialog("Heads Up", "Gotta select something before we put a script on it", "OK"))
                    return;
            }

            go.AddComponent<FV_BranchRotation>();

        }
        GUILayout.Space(guiSpace);

        if (GUILayout.Button("+ FV_Items", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            var go = Selection.activeGameObject;
            if (go == null)
            {
                if (EditorUtility.DisplayDialog("Heads Up", "Gotta select something before we put a script on it", "OK"))
                    return;
            }

            go.AddComponent<FV_Items>();

        }
        GUILayout.Space(guiSpace);

        if (GUILayout.Button("Screen Shot", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            FV_Screenshots.ShowWindow();
        }
        GUILayout.Space(guiSpace);

        if (GUILayout.Button("Tree Tools", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            FV_TreeTools.ShowWindow();

        }
        GUILayout.Space(guiSpace);

        GUILayout.EndVertical();

    }


}
#endif
