using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    //Menu Item + Hot Key + Init Window
    [MenuItem("Custom/Custom  %#]")]
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
        //Tabs
        toolbarInt = GUI.Toolbar(new Rect(3, 3, position.width - 6, 25), toolbarInt, new string[] {"基本元素", " 1", " 2"});

        switch (toolbarInt)
        {
            case 0:
                
                EditorGUILayout.Space(30);
                
                GUILayout.Label("This is a Label");
        
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
                if (GUILayout.Button("With Cancel"))
                    EditorUtility.DisplayDialog("This is Title", "This is Message", "OK Text", "Cancel Text");
                
                if (GUILayout.Button("No Cancel"))
                    EditorUtility.DisplayDialog("This is Title", "This is Message", "OK Text");
                
                EditorGUILayout.Space(10);
                
                //Progress Bar
                EditorGUILayout.LabelField("Progress Bar");
                if (GUILayout.Button("Start Progress Bar"))
                    EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", 0.5f);
                
                if (GUILayout.Button("End Progress Bar"))
                    EditorUtility.ClearProgressBar();

                break;
            
            case 1:
                
                EditorGUILayout.Space(30);
                
                    
                break;
            
            case 2:
                
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
    
    /*
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
    */
}
