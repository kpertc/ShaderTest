using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Follower system base on https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html

[ExecuteAlways]
public class smoothLerp : MonoBehaviour 
{
    [System.Serializable] // For shown in inspector
    public struct followerData {
        public GameObject obj;
        public Vector3 Offset;
    }

    public List<followerData> followers;

    [Range(0f, 1f)] public float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    public bool isDrawBeizerCurve = true;

    void Update() {
        foreach (followerData follower in followers)
        {
            Vector3 targetPosition = transform.TransformPoint(follower.Offset);

            follower.obj.transform.position = Vector3.SmoothDamp(follower.obj.transform.position, targetPosition, ref velocity, smoothTime);
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
    }
}