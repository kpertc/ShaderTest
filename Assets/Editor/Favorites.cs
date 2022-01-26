using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using System;

public class Favorites : EditorWindow
{
    [MenuItem("Custom/EditorWindows/Favorites")]
    static void Init() => GetWindow<Favorites>("Favorites");


    public favoritesResources _favoritesResourcesConfig;
    public int toolbarInt;


    private void OnEnable()
    {
        _favoritesResourcesConfig = (favoritesResources)AssetDatabase.LoadAssetAtPath("Assets/Script/Scriptable Assets/Favorites Resources.asset", typeof(favoritesResources));

    }

    private void OnGUI()
    {

        GUILayout.Label("Config Profile", EditorStyles.boldLabel);

        _favoritesResourcesConfig = (favoritesResources)EditorGUILayout.ObjectField(_favoritesResourcesConfig, typeof(favoritesResources), false);

        //Tabs
        toolbarInt = GUILayout.Toolbar(toolbarInt, new string[] { "Scenes", "Scripts", "Websites" });

        switch (toolbarInt)
        {
            case 0:
                //Looping SceneList
                for (int i = 0; i < _favoritesResourcesConfig.sceneList.Count; i++)
                {
                    EditorGUILayout.Space(5);
                    //GUILayout.Label(_favoritesResourcesConfig.sceneList[i].title, EditorStyles.boldLabel);

                    //Looping SceneList -> SceneFiles
                    for (int j = 0; j < _favoritesResourcesConfig.sceneList[i].scenefiles.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.sceneList[i].scenefiles[j].name);
                        if (GUILayout.Button("Open", GUILayout.Width(100)))
                            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_favoritesResourcesConfig.sceneList[i].scenefiles[j]), OpenSceneMode.Single);

                        GUILayout.EndHorizontal();
                    }
                }
                break;

            case 1:

                //Looping SriptList
                for (int i = 0; i < _favoritesResourcesConfig.CSharpList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    //Looping SceneList -> SceneFiles
                    for (int j = 0; j < _favoritesResourcesConfig.CSharpList[i].CSharpfiles.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.CSharpList[i].CSharpfiles[j].name);

                        if (GUILayout.Button("Show", GUILayout.Width(60)))
                            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(_favoritesResourcesConfig.CSharpList[i].CSharpfiles[j]));

                        if (GUILayout.Button("Edit", GUILayout.Width(60)))
                            AssetDatabase.OpenAsset(_favoritesResourcesConfig.CSharpList[i].CSharpfiles[j]);

                        GUILayout.EndHorizontal();
                    }
                }

                break;

            case 2:

                for (int i = 0; i < _favoritesResourcesConfig.websiteList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    //Looping SceneList -> SceneFiles
                    for (int j = 0; j < _favoritesResourcesConfig.websiteList[i].websites.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.websiteList[i].websites[j].shortName);

                        if (GUILayout.Button("Open", GUILayout.Width(60)))
                            Application.OpenURL(_favoritesResourcesConfig.websiteList[i].websites[j].urls);

                        GUILayout.EndHorizontal();
                    }
                }

                break;
        }

        var e = Event.current;

        if (e?.isKey == true)
        {
            switch (e.type)
            {
                case EventType.KeyDown:
                    switch (e.keyCode)
                    {
                        case KeyCode.LeftArrow:

                            if (toolbarInt > 0)
                            {
                                toolbarInt -= 1;
                                //Debug.Log("Current: " + toolbarInt + " Previous Page");
                                Repaint();
                            }
                            break;

                        case KeyCode.RightArrow:

                            if (toolbarInt < 2)
                            {
                                toolbarInt += 1;
                                //Debug.Log("Current: " + toolbarInt + " Next Page");
                                Repaint();
                            }
                            break;
                    }
                break;
            }
        }
    }
}

[CreateAssetMenu]
public class favoritesResources : ScriptableObject
{
    [Serializable]
    public class favoritesScenes
    {
        public string title;
        public List<SceneAsset> scenefiles;
    }

    [Serializable]
    public class favoritesCSharp
    {
        public string title;
        public List<MonoScript> CSharpfiles;
    }

    [Serializable]
    public class favoritesWebs
    {
        public string title;
        public List<website> websites;
    }

    [Serializable]
    public class website
    {
        public string shortName;
        public string urls;
    }

    public List<favoritesScenes> sceneList = new List<favoritesScenes>();

    public List<favoritesCSharp> CSharpList = new List<favoritesCSharp>();

    public List<favoritesWebs> websiteList = new List<favoritesWebs>();
}