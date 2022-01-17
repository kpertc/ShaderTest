using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    //Menu Item + Hot Key + Init Window
    [MenuItem("Custom/Custom  %#]")]
    static void Init() => GetWindow<CustomEditor>("The Title");

    //Variables
    private float someValue = 0;
    public enum things 
    {
        cat,dog,fish
    }
    
    things mythings;
    string Car_ID = "12345";

    Object _transform;

    bool foldoutState;
    
    private void OnGUI()
    {
        
        //Label
        GUILayout.Label("This is a Label");
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) //safe fucntion
        {
            //Button
            if (GUILayout.Button("This is a button"))
                Debug.Log("111");
            
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
        
        //Foldout
        foldoutState = EditorGUILayout.Foldout(foldoutState, "Foldout"); // 折叠菜单状态
        if (foldoutState)
        {
            EditorGUI.indentLevel++; // indentlevel only work for Editor Elements
            
            EditorGUILayout.LabelField("Content1");
            EditorGUILayout.LabelField("Content1");
            EditorGUILayout.LabelField("Content1");
            
            EditorGUI.indentLevel--;
        }
            
        //Progress Bar
        
        
        //HelpBox
        EditorGUILayout.HelpBox("HelpBox Type:None", MessageType.None);
        EditorGUILayout.HelpBox("HelpBox Type:Info", MessageType.Info);
        EditorGUILayout.HelpBox("HelpBox Type:Error", MessageType.Error);
        EditorGUILayout.HelpBox("HelpBox Type:Warning", MessageType.Warning);

    }
    
    /*
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
    */
}
