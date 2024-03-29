using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Follower system base on https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html

[ExecuteAlways]
[AddComponentMenu("Common Scripts/Smooth Lerp MainBody")]
public class smoothLerp : MonoBehaviour 
{
    [System.Serializable] // For shown in inspector
    public class followerData {
        public GameObject obj;
        public Vector3 Offset;
        public float distance;
        public bool isRot;
        public Vector3 movingVector;
    }

    public List<followerData> followers;

    [Range(0f, 1f)] public float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    public bool isDrawBeizerCurve = true;
    public bool isDrawLine = true;

    void Update() {

        for (int i = 0; i < followers.Capacity; i++)
        {
            Vector3 targetPosition = transform.TransformPoint(followers[i].Offset);

            followers[i].movingVector = transform.position - followers[i].obj.transform.position;

            followers[i].obj.transform.position = Vector3.SmoothDamp(followers[i].obj.transform.position, targetPosition, ref velocity, smoothTime);

            followers[i].distance = Vector3.Distance(followers[i].obj.transform.position, transform.position);

            if (followers[i].isRot) followers[i].obj.transform.eulerAngles = transform.eulerAngles;
        } 
    }

    private void OnDrawGizmos() {

        if (isDrawBeizerCurve == true) {
            foreach (followerData follower in followers) {

                Vector3 followerPos = follower.obj.transform.position;
                Vector3 offset = Vector3.up * (follower.obj.transform.position.y - transform.position.y) * 0.5f; // * half height
                Handles.DrawBezier(followerPos, transform.position, followerPos - offset, transform.position + offset, Color.white, EditorGUIUtility.whiteTexture, 1f);
            }
        }


        if (isDrawLine == true)
        {
            foreach (followerData follower in followers)
            {
                Vector3 followerPos = follower.obj.transform.position;
                Handles.DrawAAPolyLine(followerPos, transform.position);
            }
        }
    }
}