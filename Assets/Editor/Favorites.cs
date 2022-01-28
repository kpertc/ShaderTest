using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using System;

public class Favorites : EditorWindow
{
    [MenuItem("Custom/Favorites")]
    static void Init() => GetWindow<Favorites>("Favorites");

    public favoritesResources _favoritesResourcesConfig;
    public int toolbarInt;

    //UI Variables
    bool foldoutState;

    //scrollPos
    Vector2 scrollPos0;
    Vector2 scrollPos1;
    Vector2 scrollPos2;
    Vector2 scrollPos3;

    private void OnEnable()
    {
        //记住应用SO from 记住

        _favoritesResourcesConfig = (favoritesResources)AssetDatabase.LoadAssetAtPath("Assets/Script/Scriptable Assets/JingfuChen_FR.asset", typeof(ScriptableObject));

        _favoritesResourcesConfig.sceneHistoryList[0] = new favoritesResources.sceneData(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().path);

        EditorSceneManager.activeSceneChangedInEditMode += sceneHistory;
    }

    private void OnDestroy()
    {
        EditorSceneManager.activeSceneChangedInEditMode -= sceneHistory;

        //记住SO
    }

    private void sceneHistory(Scene current, Scene next)
    {
        bool exist = false; // expect not in
        foreach (favoritesResources.sceneData scene in _favoritesResourcesConfig.sceneHistoryList)
        {
            if (scene.sceneName == SceneManager.GetActiveScene().name) exist = true; // Scene has already Existed
        }

        if (!exist) // is not will add new scene
        {
            for (int i = _favoritesResourcesConfig.sceneHistoryList.Length - 1; i > 0; i--) _favoritesResourcesConfig.sceneHistoryList[i] = _favoritesResourcesConfig.sceneHistoryList[i - 1];

            _favoritesResourcesConfig.sceneHistoryList[0] = new favoritesResources.sceneData(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().path);
        }

        // 提前
    }
    
    private void OnGUI()
    {

        foldoutState = EditorGUILayout.Foldout(foldoutState, "Configs"); // 折叠菜单状态
        if (foldoutState)
        {
            EditorGUI.indentLevel++; // indentlevel only work for Editor Elements

            EditorGUILayout.LabelField("Config File: ", EditorStyles.boldLabel);

            _favoritesResourcesConfig = (favoritesResources)EditorGUILayout.ObjectField(_favoritesResourcesConfig, typeof(ScriptableObject), false);

            EditorGUILayout.Space(10);

            EditorGUI.indentLevel--;
        }


        //Tabs
        toolbarInt = GUILayout.Toolbar(toolbarInt, new string[] { "Scenes", "Scripts", "Websites", "Memo" }, GUILayout.Height(30));

        switch (toolbarInt)
        {
            case 0:

                GUILayout.Space(2);

                //Scene History Part
                scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Width(position.width), GUILayout.Height(40));

                GUILayout.BeginHorizontal();

                foreach (favoritesResources.sceneData _scene in _favoritesResourcesConfig.sceneHistoryList)
                {
                    if (GUILayout.Button(_scene.sceneName, GUILayout.Height(25), GUILayout.Width(100)))
                    {
                        EditorSceneManager.OpenScene(_scene.scenePath, OpenSceneMode.Single);
                    }
                }

                GUILayout.EndHorizontal();

                EditorGUILayout.EndScrollView();


                //Favorite Scene Part
                scrollPos0 = EditorGUILayout.BeginScrollView(scrollPos0);

                for (int i = 0; i < _favoritesResourcesConfig.sceneList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    GUILayout.Label(_favoritesResourcesConfig.sceneList[i].title, EditorStyles.boldLabel);

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
                }

                EditorGUILayout.EndScrollView();
                break;

            case 1:

                scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1);

                //Looping SriptList
                for (int i = 0; i < _favoritesResourcesConfig.CSharpList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    GUILayout.Label(_favoritesResourcesConfig.CSharpList[i].title, EditorStyles.boldLabel);

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

                EditorGUILayout.EndScrollView();

                break;

            case 2:

                scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);

                for (int i = 0; i < _favoritesResourcesConfig.websiteList.Count; i++)
                {
                    EditorGUILayout.Space(5);

                    GUILayout.Label(_favoritesResourcesConfig.websiteList[i].title, EditorStyles.boldLabel);

                    for (int j = 0; j < _favoritesResourcesConfig.websiteList[i].websites.Capacity; j++)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        GUILayout.Label(_favoritesResourcesConfig.websiteList[i].websites[j].shortName);

                        if (GUILayout.Button("Open", GUILayout.Width(60)))
                            Application.OpenURL(_favoritesResourcesConfig.websiteList[i].websites[j].address);

                        GUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.EndScrollView();

                break;

            case 3:

                _favoritesResourcesConfig.notes = GUILayout.TextArea(_favoritesResourcesConfig.notes, GUILayout.Height(position.height - 63));

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

                            if (toolbarInt < 3)
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
