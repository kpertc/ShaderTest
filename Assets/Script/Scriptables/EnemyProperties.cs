using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][Serializable]
public class EnemyProperties : ScriptableObject
{
    public int intExample;
    public float floatExample;
    public Color colorExample;
    
    public string[] stringArrayExmple;
    public List<string> stringListExample;
}
 