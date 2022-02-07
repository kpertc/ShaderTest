using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    //Menu Item + Hot Key + Init Window
    [MenuItem("Custom/EditorWindows/EditorWindowExamples  %#]")]
    static void Init() => GetWindow<CustomEditor>("The Title");

    //Variables
    private float someValue = 0;
    public enum things { cat,dog,fish }
    things mythings;
    string Car_ID = "12345";
    Object _transform;
    bool foldoutState;
    private int toolbarInt;
    
    
    private void OnGUI()
    {
        minSize = new Vector2(300, 300);
        
        //Tabs
        toolbarInt = GUI.Toolbar(new Rect(3, 3, position.width - 6, 25), toolbarInt, new string[] {"Essentials", "OnSceneGUI", "Layout", "Style"});

        switch (toolbarInt)
        {
            case 0:
                
                EditorGUILayout.Space(30);
                
                GUILayout.Label("This is a Label");
                GUILayout.Label("This is a BOLD Label", EditorStyles.boldLabel);

                using (new GUILayout.VerticalScope(EditorStyles.helpBox)) //safe fucntion
                {
                    //Button
                    if (GUILayout.Button("This is a button"))
                        Debug.Log("Button Pressed");
            
                    //Slider
                    GUILayout.BeginHorizontal(); //unSafe
            
                    GUILayout.Label("Slider: ");
                    someValue = GUILayout.HorizontalSlider(someValue, -1, 1);
            
                    GUILayout.EndHorizontal();
                    
                    //TODO MinMaxSlider
            
                    //Space
                    GUILayout.Space(20);
            
                    //EnumPop
                    mythings = (things)EditorGUILayout.EnumPopup("Choose a animal: ", mythings);
            
                    //TextField
                    Car_ID = GUILayout.TextField(Car_ID, 25);
                    GUILayout.Label("Result: " + Car_ID);
            
                    //ObjectField
                    _transform = EditorGUILayout.ObjectField("Text Example: ", null, typeof(Transform), true);
            
                }
                
                EditorGUILayout.Space(10);

                //Modular Button
                EditorGUILayout.BeginHorizontal();
                GUILayout.Button("miniButtonLeft", EditorStyles.miniButtonLeft);
                GUILayout.Button("miniButtonMid", EditorStyles.miniButtonMid);
                GUILayout.Button("miniButtonRight", EditorStyles.miniButtonRight);
                EditorGUILayout.EndHorizontal();

                //Image Buttons
                if (GUILayout.Button(Resources.Load<Texture>("TEST"),
                    GUILayout.Width(64), GUILayout.Height(64)))
                {
                    Debug.Log("image button");
                }

                //PrefixLabel
                EditorGUILayout.LabelField("PrefixLabel");
                int ammo = 10;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Ammo");  //Label to show to the left of the control.
                ammo = EditorGUILayout.IntField(ammo);
                EditorGUILayout.EndHorizontal();
                                //Foldout
                foldoutState = EditorGUILayout.Foldout(foldoutState, "Foldout"); // 折叠菜单状态
                if (foldoutState)
                {
                    EditorGUI.indentLevel++; // indentlevel only work for Editor Elements
            
                    EditorGUILayout.LabelField("LabelField1");
                    EditorGUILayout.LabelField("LabelField2");
                    EditorGUILayout.LabelField("LabelField3");
            
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.Space(10);
                
                //Display Dialog
                EditorGUILayout.LabelField("Dialog");
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("With Cancel"))
                    EditorUtility.DisplayDialog("This is Title", "This is Message", "OK Text", "Cancel Text");
                
                if (GUILayout.Button("No Cancel"))
                    EditorUtility.DisplayDialog("This is Title", "This is Message", "OK Text");
                GUILayout.EndHorizontal();
                
                EditorGUILayout.Space(10);
                
                //Progress Bar (Popup Window)
                EditorGUILayout.LabelField("Progress Bar (Popup)");
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Start Progress Bar"))
                    EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", 0.5f);
                
                if (GUILayout.Button("End Progress Bar"))
                    EditorUtility.ClearProgressBar();
                GUILayout.EndHorizontal();
                //EditorGUILayout.Separator();
                
                EditorGUILayout.Space(10);
                
                //Progress Bar
                EditorGUILayout.LabelField("Progress Bar (UI)");
                Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField"); //Use rect as Layout Base
                EditorGUI.ProgressBar(rect, 0.5f, "Sample Text");

                break;
            
            case 1:
                
                EditorGUILayout.Space(30);
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Enable OnSceneGUI"))
                    SceneView.duringSceneGui += OnSceneGUI;
                    SceneView.RepaintAll();
                
                if (GUILayout.Button("Disable OnSceneGUI"))
                    SceneView.duringSceneGui -= OnSceneGUI;
                    SceneView.RepaintAll();
                
                GUILayout.EndHorizontal();

                break;
            
            case 2:
                EditorGUILayout.Space(30);
                
                EditorGUILayout.LabelField("Text1", EditorStyles.helpBox);

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Text1");
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Text2");
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField("Text3", EditorStyles.helpBox);
                
                break;
            
            case 3:
                
                EditorGUILayout.Space(30);
                
                //HelpBox
                EditorGUILayout.HelpBox("HelpBox Type:None", MessageType.None);
                EditorGUILayout.HelpBox("HelpBox Type:Info", MessageType.Info);
                EditorGUILayout.HelpBox("HelpBox Type:Error", MessageType.Error);
                EditorGUILayout.HelpBox("HelpBox Type:Warning", MessageType.Warning);
        
                EditorGUILayout.Space(20);
                    
                break;
        }
    }
    
    void OnSceneGUI(SceneView sceneview)
    {
        Handles.BeginGUI();
 
        if (GUILayout.Button("Button"))
            Debug.Log("Button Clicked");
 
        Handles.EndGUI();
    }
    
    /*
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
    */
}
