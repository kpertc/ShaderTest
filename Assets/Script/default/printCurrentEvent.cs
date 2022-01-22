using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printCurrentEvent : MonoBehaviour
{
    void OnGUI()
    {
        if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
        {
            Debug.Log("Current event detected: " + Event.current.type);
        }
    }
}
