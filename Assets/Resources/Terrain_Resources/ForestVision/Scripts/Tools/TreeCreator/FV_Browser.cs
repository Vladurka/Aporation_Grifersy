using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;


namespace ForestVision.FV_TreeEditor
{
    public class FV_Browser : EditorWindow
    {
        public static FV_Browser instance;
        private List<FV_Items.Category> _categories;
        private List<string> _categoryLabels;
        private FV_Items.Category _categorySelected;

        static private string _path = "Assets/ForestVision";

        private List<FV_Items> _items;
        private Dictionary<FV_Items.Category, List<FV_Items>> _categorizedItems;
        private Dictionary<FV_Items, Texture2D> _previews;
        private Vector2 _scrollPosition;
        private const float ButtonWidth = 140;
        private const float ButtonHeight = 150;

        public delegate void itemSelectedDelegate(FV_Items item, Texture2D preview);

        public static event itemSelectedDelegate ItemSelectedEvent;


        private void Awake()
        {
            instance = this;
        }
        private void OnEnable()
        {
            if (_categories == null)
                InitCategories();

            if (_categorizedItems == null)
                InitContent();
        }

        public void InitCategories()
        {
            _categories = FV_edUtils.GetListFromEnum<FV_Items.Category>();
            _categoryLabels = new List<string>();
            foreach (FV_Items.Category category in _categories)
                _categoryLabels.Add(category.ToString());

        }

        public static void InitContent()
        {
            // Set the ScrollList
            instance._items = new List<FV_Items>();
            instance._items = FV_edUtils.GetAssetsWithScript<FV_Items>(_path);

            instance._categorizedItems = new Dictionary<FV_Items.Category, List<FV_Items>>();
            instance._previews = new Dictionary<FV_Items, Texture2D>();
            // Init the Dictionary
            foreach (FV_Items.Category category in instance._categories)
                instance._categorizedItems.Add(category, new List<FV_Items>());

            // Assign items to each category 
            foreach (FV_Items item in instance._items)
                instance._categorizedItems[item.category].Add(item);

            GeneratePreviews();
        }

        private void DrawScroll()
        {

            if (_categorizedItems?[_categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This category is empty!", MessageType.Info);
                return;
            }

            int rowCapacity = Mathf.FloorToInt(position.width / (ButtonWidth));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            int selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(
                selectionGridIndex,
                GetGUIContentsFromItems(),
                rowCapacity,
                GetGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }

        private void DrawTabs()
        {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoryLabels.ToArray());
            _categorySelected = _categories[index];
        }

        public static void GeneratePreviews()
        {
            AssetPreview.SetPreviewTextureCacheSize(1024);
            foreach (FV_Items item in instance._items)
            {
                if (!instance._previews.ContainsKey(item))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item.gameObject);
                    if (preview != null)
                        instance._previews.Add(item, preview);
                }
            }
        }

        public static void ShowEditor()
        {
            instance = (FV_Browser)EditorWindow.GetWindow(typeof(FV_Browser));
            instance.titleContent = new GUIContent("ForestVision Browser");
            instance.autoRepaintOnSceneChange = true;
            instance.titleContent = new GUIContent("FV Browser");
        }
        public static void ResetTransformOnSelected(GameObject selObj)
        {
            GameObject newParent = new GameObject();
            newParent.name = selObj.name;
            newParent.transform.position = new Vector3(0, 0, 0);
            selObj.transform.SetParent(newParent.transform);
        }



        private void OnGUI()
        {

            EditorGUILayout.LabelField("ForestVision Asset Collection Browser", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (GUILayout.Button("Refresh Thumbnails", GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)))
            {
                InitContent();
            }

            DrawTabs();
            EditorGUILayout.HelpBox("Select an asset to add it to the scene. Newly created objects will by default be the child of anything selected. New objects will be placed at (0,0,0) of its parent object.", MessageType.None);
            DrawScroll();
        }

        private GUIContent[] GetGUIContentsFromItems()
        {
            List<GUIContent> guiContents = new List<GUIContent>();

            int totalItems = _categorizedItems?.Count > 0 ? _categorizedItems[_categorySelected].Count : 0;
            for (int i = 0; i < totalItems; i++)
            {
                GUIContent guiContent = new GUIContent();
                guiContent.text = _categorizedItems[_categorySelected][i].itemName;

                if (_previews[_categorizedItems[_categorySelected][i]] != null)
                {
                    guiContent.image = _previews[_categorizedItems[_categorySelected][i]];
                }

                guiContents.Add(guiContent);
            }

            return guiContents.ToArray();
        }

        private GUIStyle GetGUIStyle()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = ButtonWidth;
            guiStyle.fixedHeight = ButtonHeight;
            return guiStyle;
        }

        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                FV_Items selectedItem = _categorizedItems[_categorySelected][index];
                //Debug.Log("GetSelectedItem: " + _categorizedItems[_categorySelected].Count);
                GameObject obj = PrefabUtility.InstantiatePrefab(selectedItem.gameObject) as GameObject;
                obj.name = "FV_" + selectedItem.itemName;
                if (Selection.activeTransform != null)
                {
                    obj.transform.parent = Selection.activeTransform;
                    obj.transform.localPosition = Vector3.zero;
                }

                if (ItemSelectedEvent != null)
                {
                    ItemSelectedEvent(selectedItem, _previews[selectedItem]);
                }
            }
        }


    }
}
#endif