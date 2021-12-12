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
    }
}
