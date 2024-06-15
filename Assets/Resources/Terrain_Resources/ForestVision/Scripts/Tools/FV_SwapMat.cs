using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


[ExecuteInEditMode]
public class FV_SwapMat : EditorWindow
{
    public Material swapMat = null;
    private string matName;
    private int childCountWithSameMat = 0;

    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(FV_SwapMat));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Swap Material");
    }

    void OnGUI()
    {
        GUILayout.Label("What Material do you want to swap with?", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        swapMat = (Material)EditorGUILayout.ObjectField(swapMat, typeof(Material), true) as Material;
        if (swapMat)
            matName = swapMat.name;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        GUILayout.Label($"Found:{GetMaterialCount()} valid objects to swap based on your material choice", EditorStyles.wordWrappedLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Swap Material on all selected", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
        {
            var go = Selection.activeGameObject;
            if (go == null)
                if (EditorUtility.DisplayDialog("Heads Up", "Can't swap materials without something selected", "OK"))
                    return;

            SwapMatOnSelected();
        }
    }

    private void CompareMatNamesAndSwapMaterial(GameObject sel)
    {
        if (sel.GetComponent<MeshRenderer>() != null)
        {
            string[] compareString = matName.Split('_');
            bool isSameMatCategory = sel.GetComponent<MeshRenderer>().sharedMaterial.name.Contains(compareString[0]);
            if (isSameMatCategory)
                SwapMat(sel);
        }
    }

    private void CompareMatNamesAndCount(GameObject sel)
    {
        string[] compareString = matName.Split('_');
        if (sel.GetComponent<MeshRenderer>() != null)
        {
            bool isSameMatCategory = sel.GetComponent<MeshRenderer>().sharedMaterial.name.Contains(compareString[0]);
            if (isSameMatCategory)
                childCountWithSameMat++;
        }
    }

    private int GetMaterialCount()
    {
        childCountWithSameMat = 0;
        foreach (GameObject obj in Selection.gameObjects)
        {
            CompareMatNamesAndCount(obj);

            if (obj.transform.childCount > 0)
                LoopThroughChildrenCount(obj);

        }

        return childCountWithSameMat;
    }

    private void SwapMat(GameObject thisGameObject)
    {
        // handle the current selection
        MeshRenderer mr = thisGameObject.transform.GetComponent<MeshRenderer>();
        // if this gameobject has a mesh renderer assigned, handle the swap
        if (mr != null)
            mr.material = swapMat;
    }

    public void LoopThroughChildrenCount(GameObject currentGameObj)
    {

        // we know for certain we have children at this point, so loop through them
        // and decide if we need to switch out materials
        for (int i = 0; i < currentGameObj.transform.childCount; i++)
        {
            CompareMatNamesAndCount(currentGameObj.transform.GetChild(i).gameObject);

            if (currentGameObj.transform.GetChild(i).transform.childCount > 0)
                LoopThroughChildrenCount(currentGameObj.transform.GetChild(i).gameObject);

        }
    }


    public void LoopThroughChildren(GameObject currentGameObj)
    {
        // we know for certain we have children at this point, so loop through them
        // and decide if we need to switch out materials
        for (int i = 0; i < currentGameObj.transform.childCount; i++)
        {
            CompareMatNamesAndSwapMaterial(currentGameObj.transform.GetChild(i).gameObject);

            if (currentGameObj.transform.GetChild(i).transform.childCount > 0)
                LoopThroughChildren(currentGameObj.transform.GetChild(i).gameObject);

        }
    }

    public void SwapMatOnSelected()
    {
        // run through all of the children of the current selection
        foreach (GameObject g in Selection.gameObjects)
        {
            // compare main selection first
            CompareMatNamesAndSwapMaterial(g);
            // now dive into children, if any
            if (g.transform.childCount > 0)
                LoopThroughChildren(g);
        }

    }
}

#endif