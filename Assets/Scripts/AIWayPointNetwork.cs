using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode { None, Connections, Paths }
public class AIWayPointNetwork : MonoBehaviour
{
    
    public PathDisplayMode displayMode = PathDisplayMode.Connections;
    [HideInInspector]
    public int Start = 0;
    [HideInInspector]
    public int End = 0;


    public List<Transform> wayPoints = new List<Transform>();
}
