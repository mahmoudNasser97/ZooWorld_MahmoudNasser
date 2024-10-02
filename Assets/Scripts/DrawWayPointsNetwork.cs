using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.AI;

[CustomEditor(typeof(AIWayPointNetwork))]
public class DrawWayPointsNetwork : Editor
{
    public override void OnInspectorGUI()
    {
        AIWayPointNetwork network = (AIWayPointNetwork)target;

        network.displayMode = (PathDisplayMode)(EditorGUILayout.EnumPopup("Display Mode ", network.displayMode));
        if (network.displayMode == PathDisplayMode.Paths)
        {
            network.Start = EditorGUILayout.IntSlider("WayPoint Start", network.Start, 0, network.wayPoints.Count - 1);
            network.End = EditorGUILayout.IntSlider("WayPoint End", network.End, 0, network.wayPoints.Count - 1);
        }
        DrawDefaultInspector();
    }
    private void OnSceneGUI()
    {
        AIWayPointNetwork network = (AIWayPointNetwork)target;
        for (int i = 0; i < network.wayPoints.Count; i++)
        {
            if (network.wayPoints[i] != null)
                Handles.Label(network.wayPoints[i].position, "WayPoint   " + i.ToString());
        }
        if (network.displayMode == PathDisplayMode.Connections)
        {


            Vector3[] linePoints = new Vector3[network.wayPoints.Count + 1];
            for (int i = 0; i < network.wayPoints.Count; i++)
            {
                int index = i != network.wayPoints.Count ? i : 0;
                if (network.wayPoints[index] != null)
                    linePoints[i] = network.wayPoints[index].position;
                else
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }
            Handles.color = Color.red;
            Handles.DrawPolyLine(linePoints);
        }
        else
        {
            if (network.displayMode == PathDisplayMode.Paths)
            {
                NavMeshPath path = new NavMeshPath();

                if (network.wayPoints[network.Start] != null && network.wayPoints[network.End] != null)
                {
                    Vector3 from = network.wayPoints[network.Start].position;
                    Vector3 to = network.wayPoints[network.End].position;

                    NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
                    Handles.color = Color.yellow;
                    Handles.DrawPolyLine(path.corners);
                }

            }
        }

    }
}
