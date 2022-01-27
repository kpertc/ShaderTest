using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System;

public class Favorites : EditorWindow
{
    [MenuItem("Custom/EditorWindows/Favorites")]
    static void Init() => GetWindow<Favorites>("Favorites");


    public favoritesResources _favoritesResourcesConfig;
    public int toolbarInt;

    public static int historySize = 6;
    public sceneData[] sceneHistoryList = new sceneData[historySize];

    //scrollPos
    Vector2 scrollPos0;
    Vector2 scrollPos1;
    Vector2 scrollPos2;
    Vector2 scrollPos3;


    private void OnEnable()
    {
        _favoritesResourcesConfig = (favoritesResources)AssetDatabase.LoadAssetAtPath("Assets/Script/Scriptable Assets/Favorites Resources.asset", typeof(favoritesResources));

        sceneHistoryList[0] = new sceneData(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().path);

        EditorSceneManager.activeSceneChangedInEditMode += sceneHistory;

    }

    private void OnDestroy()
    {
        EditorSceneManager.activeSceneChangedInEditMode -= sceneHistory;
    }

    public struct sceneData
    {
        public string sceneName;
        public string scenePath;

        public sceneData (string _sceneName, string _scenePath)
        {
            this.sceneName = _sceneName;
            this.scenePath = _scenePath;
        }
    }

    private void sceneHistory(Scene current, Scene next)
    {
        bool exist = false; // expect not in
        foreach (sceneData scene in sceneHistoryList )
        {
            if (scene.sceneName == SceneManager.GetActiveScene().name)
            {
                exist = true; // Scene has already Existed
            }
        }

        if (!exist) // is not will add new scene
        {
            for (int i = historySize - 1; i > 0; i--) sceneHistoryList[i] = sceneHistoryList[i - 1];

            sceneHistoryList[0] = new sceneData(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().path);
        }

        // 提前
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

                GUILayout.Space(2);

                scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Width(position.width), GUILayout.Height(40));

                GUILayout.BeginHorizontal();

                foreach (sceneData _scene in sceneHistoryList)
                {
                    if (GUILayout.Button(_scene.sceneName, GUILayout.Height(25), GUILayout.Width(100)))
                    {
                        EditorSceneManager.OpenScene(_scene.scenePath, OpenSceneMode.Single);
                    }
                }

                GUILayout.EndHorizontal();

                EditorGUILayout.EndScrollView();

                //Looping SceneList
                    for (int i = 0; i < _favoritesResourcesConfig.sceneList.Count; i++)
                {
                    EditorGUILayout.Space(5);
                    //GUILayout.Label(_favoritesResourcesConfig.sceneList[i].title, EditorStyles.boldLabel);

                    scrollPos0 = EditorGUILayout.BeginScrollView(scrollPos0);
                    //Looping SceneList -> SceneFiles
                    for (int j = 0; j < _favoritesResourcesConfig.sceneList[i].scenefiles.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.sceneList[i].scenefiles[j].name);
                        if (GUILayout.Button("Show", GUILayout.Width(60)))
                            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(_favoritesResourcesConfig.sceneList[i].scenefiles[j]));

                        if (GUILayout.Button("Open", GUILayout.Width(60)))
                            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_favoritesResourcesConfig.sceneList[i].scenefiles[j]), OpenSceneMode.Single);

                        GUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                }
                break;

            case 1:

                //Looping SriptList
                for (int i = 0; i < _favoritesResourcesConfig.CSharpList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1);
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
                    EditorGUILayout.EndScrollView();
                }

                break;

            case 2:

                for (int i = 0; i < _favoritesResourcesConfig.websiteList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
                    //Looping SceneList -> SceneFiles
                    for (int j = 0; j < _favoritesResourcesConfig.websiteList[i].websites.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.websiteList[i].websites[j].shortName);

                        if (GUILayout.Button("Open", GUILayout.Width(60)))
                            Application.OpenURL(_favoritesResourcesConfig.websiteList[i].websites[j].urls);

                        GUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
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