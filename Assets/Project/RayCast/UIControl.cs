using System;
using UnityEngine;
using DG.Tweening;

public class UIControl : MonoBehaviour
{
    public UIType _UIType;
    [Tooltip("Animtion Transition Duration")]public float animTime = 0.1f;
    
    [Header("Hover Effect Control")]
    public bool enableSnapping;
    public bool enableHightlight;
    [Space(10)]
    public bool enableOutline;
    public float enableOutlineIdle;
    public float enableOutlineHover = 0.08f;

    [Space(10)]
    public bool enableTranslation;
    public float translationSensitivity = 0.4f;
    
    [Space(5)]
    public bool enableRotation;
    public float rotationSensitivity = 3f;
    
    [Space(10)]
    [Header("Internal Infomation (ReadOnly)")]
    [readOnly]public Vector3 initPos;
    
    void Start()
    {
        initPos = transform.position;
        GetComponent<Renderer>().material.SetFloat("_OutlineWidth", enableOutlineIdle);
    }


    public enum UIType
    {
        UI,
        Panel
    };
}
