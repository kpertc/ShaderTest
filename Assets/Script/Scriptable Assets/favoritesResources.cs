using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CreateAssetMenu]
public class favoritesResources : ScriptableObject
{
    [Serializable]
    public struct sceneData
    {
        public string sceneName;
        public string scenePath;

        public sceneData(string _sceneName, string _scenePath)
        {
            this.sceneName = _sceneName;
            this.scenePath = _scenePath;
        }
    }

    public favoritesResources.sceneData[] sceneHistoryList;

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


    // Favorite Folders
    /*
    [Serializable]
    public class favoritesWebs
    {
        public string title;
        public List<website> websites;
    }

    [Serializable]
    public struct website
    {
        public string shortName;
        public string address;
    }
    */



    [Serializable]
    public class favoritesWebs
    {
        public string title;
        public List<website> websites;
    }

    [Serializable]
    public struct website
    {
        public string shortName;
        public string address;
    }



    public List<favoritesScenes> sceneList = new List<favoritesScenes>();

    public List<favoritesCSharp> CSharpList = new List<favoritesCSharp>();

    public List<favoritesWebs> websiteList = new List<favoritesWebs>();

    public string notes;
}
