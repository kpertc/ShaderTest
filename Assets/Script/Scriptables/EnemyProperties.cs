using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyProperties : ScriptableObject
{
    public Color characterColor = Color.red;

    public float damage = 10;
    public float defense = 5;

    public float radius = 5;
}
 