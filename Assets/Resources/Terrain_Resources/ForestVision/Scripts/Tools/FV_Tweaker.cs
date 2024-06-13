using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;


[ExecuteInEditMode]

public class FV_Tweaker : EditorWindow
{
    private bool tweakScaling;
    private float scaleAmount = 1;
    private float oldScaleAmount = 1;
    private float rotationAmount = 1;
    private float oldRotationAmount = 1;
    private float positionAmount = 1;
    private float oldPositionAmount = 1;
    int randomAmount;
    bool localSpace = false;
    bool justScaleSelected = false;
    bool justSelected = false;
    private bool tweakPosition;
    private bool tweakRotation;
    public enum rotAxis
    {
        x,
        y,
        z
    }
    rotAxis curAxis = rotAxis.y;
    private Vector2 windowScroll;

    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(FV_Tweaker));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Tree Tweaker");

    }


    void OnGUI()
    {
        windowScroll = EditorGUILayout.BeginScrollView(windowScroll);
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        EditorGUILayout.Space();
        if (Selection.activeGameObject)
        {
            GUILayout.Label("Switch Species on Selected", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            Vector3 defaultScale = new Vector3(1, 1, 1);
            if (Selection.activeGameObject.transform.localScale != defaultScale)
                if (GUILayout.Button("Reset Transform?", GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true)))
                {

                    GameObject newParent = new GameObject("_" + Selection.activeGameObject.name);
                    // parent the new go to the selection
                    newParent.transform.parent = Selection.activeGameObject.transform;
                    // zero its position
                    newParent.transform.localPosition = Vector3.zero;
                    // unparent
                    newParent.transform.parent = null;
                    // make sure its scale is reset
                    newParent.transform.localScale = defaultScale;
                    Selection.activeGameObject.transform.parent = newParent.transform;
                    // select the new object
                    Selection.activeGameObject = newParent;
                }
            EditorGUILayout.Space();

            EditorGUILayout.Space();

            #region Hierarchy Selections

            GUILayout.Label("Heirarchy Selctions", EditorStyles.boldLabel);
            // Handle Parent selection options
            bool hasParent = Selection.activeGameObject.transform.parent != null;
            EditorGUILayout.BeginHorizontal();
            if (hasParent)
            {

                EditorGUILayout.Space();
                if (GUILayout.Button("Select Parent?", GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true)))
                    Selection.activeGameObject = Selection.activeGameObject.transform.parent.gameObject;

                EditorGUILayout.Space();
            }

            // Handle Child Selection options
            bool hasChildren = Selection.activeGameObject.transform.childCount > 0;
            if (hasChildren)
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Select Children?", GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true)))
                {

                    List<GameObject> children = new List<GameObject>();
                    foreach (Transform child in Selection.activeGameObject.transform)
                        children.Add(child.gameObject);

                    GameObject[] gOs = children.ToArray();
                    Selection.objects = gOs;
                }
                EditorGUILayout.Space();
            }
            else
            {
                GUILayout.Label("No children exist for this object", EditorStyles.label);
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.Space();

            #region Tweaker
            GUILayout.Label("Tweak", EditorStyles.boldLabel);
            tweakPosition = EditorGUILayout.Toggle("Tweak Position?", tweakPosition);
            if (tweakPosition)
            {
                positionAmount = EditorGUILayout.Slider("Tweak Amount", positionAmount, -2, 2);
                if (positionAmount != oldPositionAmount)
                    RePosition(positionAmount);
                oldPositionAmount = positionAmount;

            }

            tweakRotation = EditorGUILayout.Toggle("Tweak Rotations?", tweakRotation);
            if (tweakRotation)
            {
                rotationAmount = EditorGUILayout.Slider("Tweak Amount", rotationAmount, 0, 1);
                if (rotationAmount != oldRotationAmount)
                    RotateSelected(rotationAmount);
                oldRotationAmount = rotationAmount;

            }

            tweakScaling = EditorGUILayout.Toggle("Tweak Scaling?", tweakScaling);
            if (tweakScaling)
            {
                scaleAmount = EditorGUILayout.Slider("Tweak Amount", scaleAmount, 0.5f, 2);
                if (scaleAmount != oldScaleAmount)
                    ScaleAll(scaleAmount);
                oldScaleAmount = scaleAmount;

            }

            #endregion

            EditorGUILayout.Space();

            randomAmount = (int)EditorGUILayout.Slider("RandomAmount", randomAmount, 0, 90);


            if (GUILayout.Button("Randomize Branches", GUILayout.MinHeight(40)))
            {
                RandomizeAllBranches();
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rotate All Branches on the following axis:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            curAxis = (rotAxis)EditorGUILayout.EnumPopup("Rotation Axis:", curAxis);
            justSelected = EditorGUILayout.Toggle("Rotate Just Selected", justSelected);
            localSpace = EditorGUILayout.Toggle("Rotate in Local", localSpace);
            EditorGUILayout.Space();

            #region Rotations 1
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+1"))
            {
                if (justSelected)
                    RotateChildren(1, false, curAxis, localSpace);

                RotateChildren(1, true, curAxis, localSpace);
            }
            if (GUILayout.Button("+5"))
            {
                if (justSelected)
                    RotateChildren(5, false, curAxis, localSpace);

                RotateChildren(5, true, curAxis, localSpace);
            }
            if (GUILayout.Button("+10"))
            {
                if (justSelected)
                    RotateChildren(10, false, curAxis, localSpace);

                RotateChildren(10, true, curAxis, localSpace);
            }
            if (GUILayout.Button("+45"))
            {
                if (justSelected)
                    RotateChildren(45, false, curAxis, localSpace);

                RotateChildren(45, true, curAxis, localSpace);
            }
            if (GUILayout.Button("+90"))
            {
                if (justSelected)
                    RotateChildren(90, false, curAxis, localSpace);

                RotateChildren(90, true, curAxis, localSpace);
            }
            if (GUILayout.Button("+180"))
            {
                if (justSelected)
                    RotateChildren(180, false, curAxis, localSpace);

                RotateChildren(180, true, curAxis, localSpace);
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Rotations 2
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-1"))
            {
                if (justSelected)
                    RotateChildren(-1, false, curAxis, localSpace);

                RotateChildren(-1, true, curAxis, localSpace);
            }
            if (GUILayout.Button("-5"))
            {
                if (justSelected)
                    RotateChildren(-5, false, curAxis, localSpace);

                RotateChildren(-5, true, curAxis, localSpace);
            }
            if (GUILayout.Button("-10"))
            {
                if (justSelected)
                    RotateChildren(-10, false, curAxis, localSpace);

                RotateChildren(-10, true, curAxis, localSpace);
            }
            if (GUILayout.Button("-45"))
            {
                if (justSelected)
                    RotateChildren(-45, false, curAxis, localSpace);

                RotateChildren(-45, true, curAxis, localSpace);
            }
            if (GUILayout.Button("-90"))
            {
                if (justSelected)
                    RotateChildren(-90, false, curAxis, localSpace);

                RotateChildren(-90, true, curAxis, localSpace);
            }
            if (GUILayout.Button("-180"))
            {
                if (justSelected)
                    RotateChildren(-180, false, curAxis, localSpace);

                RotateChildren(-180, true, curAxis, localSpace);
            }
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Space();
            EditorGUIUtility.labelWidth = 130;
            EditorGUILayout.LabelField("Scale All Branches Uniformly:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            justScaleSelected = EditorGUILayout.Toggle("Scale Just Selected", justScaleSelected);

            #region Scaling
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("0.25"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(0.25f, false);

                ScaleAllBranches(0.25f, true);
            }
            if (GUILayout.Button("0.5"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(0.5f, false);

                ScaleAllBranches(0.5f, true);
            }
            if (GUILayout.Button("0.9"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(0.9f, false);

                ScaleAllBranches(0.9f, true);
            }
            if (GUILayout.Button("1.1"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(1.1f, false);

                ScaleAllBranches(1.1f, true);
            }
            if (GUILayout.Button("1.25"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(1.25f, false);

                ScaleAllBranches(1.25f, true);
            }
            if (GUILayout.Button("1.5"))
            {
                if (justScaleSelected)
                    ScaleAllBranches(1.5f, false);

                ScaleAllBranches(1.5f, true);
            }
            EditorGUILayout.EndHorizontal();

            #endregion










        }
        else
        {
            GUILayout.Label("Select something to see your options", EditorStyles.boldLabel);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }


    void ScaleAllBranches(float scaleAmount, bool children)
    {
        Vector3 newScale;

        //if(children){
        foreach (GameObject g in Selection.gameObjects)
        {
            for (int i = 0; i < g.transform.childCount; i++)
            {
                newScale.x = scaleAmount; newScale.y = scaleAmount; newScale.z = scaleAmount;
                g.transform.GetChild(i).localScale = Vector3.Scale((new Vector3(newScale.x, newScale.y, newScale.z)), g.transform.GetChild(i).localScale);

            }

        }

    }

    private void RotateChildren(float deg, bool children, rotAxis axis, bool space)
    {
        if (children)
        {
            foreach (GameObject g in Selection.gameObjects)
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    switch ((int)axis)
                    {
                        case 0:
                            if (space)
                            {
                                g.transform.GetChild(i).Rotate(deg, 0, 0, Space.Self);
                            }
                            else
                            {
                                g.transform.GetChild(i).Rotate(deg, 0, 0, Space.World);
                            }
                            break;
                        case 1:
                            if (space)
                            {
                                g.transform.GetChild(i).Rotate(0, deg, 0, Space.Self);
                            }
                            else
                            {
                                g.transform.GetChild(i).Rotate(0, deg, 0, Space.World);
                            }
                            break;
                        case 2:
                            if (space)
                            {
                                g.transform.GetChild(i).Rotate(0, 0, deg, Space.Self);
                            }
                            else
                            {
                                g.transform.GetChild(i).Rotate(0, 0, deg, Space.World);
                            }
                            break;

                    }

                }

            }
        }
        else
        {
            switch ((int)axis)
            {
                case 0:
                    if (space)
                    {
                        Selection.activeGameObject.transform.Rotate(deg, 0, 0, Space.Self);
                    }
                    else
                    {
                        Selection.activeGameObject.transform.Rotate(deg, 0, 0, Space.World);
                    }
                    break;
                case 1:
                    if (space)
                    {
                        Selection.activeGameObject.transform.Rotate(0, deg, 0, Space.Self);
                    }
                    else
                    {
                        Selection.activeGameObject.transform.Rotate(0, deg, 0, Space.World);
                    }
                    break;
                case 2:
                    if (space)
                    {
                        Selection.activeGameObject.transform.Rotate(0, 0, deg, Space.Self);
                    }
                    else
                    {
                        Selection.activeGameObject.transform.Rotate(0, 0, deg, Space.World);
                    }
                    break;

            }
            Selection.activeGameObject.transform.Rotate(0, deg, 0, Space.Self);
        }
    }




    void RePosition(float offset)
    {
        Vector3 newPos;

        foreach (GameObject g in Selection.gameObjects)
        {
            float randomValue = UnityEngine.Random.Range(0.1f, 0.4f);
            newPos.x = offset * randomValue + UnityEngine.Random.Range(-0.5f, 0.5f);
            newPos.y = UnityEngine.Random.Range(-0.2f, 0.2f);
            newPos.z = offset * randomValue + UnityEngine.Random.Range(-0.5f, 0.5f);
            g.transform.position = g.transform.position + new Vector3(newPos.x, newPos.y, newPos.z);

        }

    }

    void RotateSelected(float rotationAmount)
    {
        Vector3 newRotation;

        foreach (GameObject g in Selection.gameObjects)
        {
            newRotation.x = UnityEngine.Random.Range(-7f, 7f) * rotationAmount;
            newRotation.y = UnityEngine.Random.Range(-90f, 90f) * rotationAmount;
            newRotation.z = UnityEngine.Random.Range(-7f, 7f) * rotationAmount;
            g.transform.localRotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);

        }

    }

    void ScaleAll(float scaleAmount)
    {
        Vector3 newScale;

        foreach (GameObject g in Selection.gameObjects)
        {
            float randomValue = UnityEngine.Random.Range(0.93f, 1.7f);
            newScale.x = scaleAmount * randomValue;
            newScale.y = scaleAmount * randomValue;
            newScale.z = scaleAmount * randomValue;
            g.transform.localScale = new Vector3(newScale.x, newScale.y, newScale.z);

        }

    }

    public void LoopThroughChildren(GameObject currentGameObj)
    {

        // we know for certain we have children at this point, so loop through them
        // and decide if we need to switch out meshes
        for (int i = 0; i < currentGameObj.transform.childCount; i++)
        {
            //SwitchSpecies(currentGameObj.transform.GetChild(i).gameObject);

            if (currentGameObj.transform.GetChild(i).transform.childCount > 0)
            {
                LoopThroughChildren(currentGameObj.transform.GetChild(i).gameObject);
            }
        }
    }

    public void LoopThroughChildren(GameObject currentGameObj, int newValue)
    {

        // we know for certain we have children at this point, so loop through them
        // and decide if we need to switch out meshes
        for (int i = 0; i < currentGameObj.transform.childCount; i++)
        {
            //do something

            if (currentGameObj.transform.GetChild(i).transform.childCount > 0)
            {
                LoopThroughChildren(currentGameObj.transform.GetChild(i).gameObject);
            }
        }
    }


    void RandomizeAllBranches()
    {
        foreach (GameObject g in Selection.gameObjects)
            for (int i = 0; i < g.transform.childCount; i++)
                g.transform.GetChild(i).Rotate(UnityEngine.Random.Range(-randomAmount, randomAmount),
                    UnityEngine.Random.Range(-randomAmount, randomAmount),
                    UnityEngine.Random.Range(-randomAmount, randomAmount),
                    Space.Self);
    }



}
#endif