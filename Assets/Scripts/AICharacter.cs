using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class AICharacter : MonoBehaviour
{
    public AIWayPointNetwork waypointNetwork = null;
    public int currentIndex = 0;
    public bool hasPath = false;
    public bool pathPending = false;
    public bool pathStale = false;
    public NavMeshPathStatus pathStatus = NavMeshPathStatus.PathInvalid;
    public AnimationCurve jumpCurve = new AnimationCurve();
    private NavMeshAgent navAgent = null;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (waypointNetwork == null) return;
        SetNextDestination(false);
    }
    void Update()
    {
        hasPath = navAgent.hasPath;
        pathPending = navAgent.pathPending;
        pathStale = navAgent.isPathStale;
        pathStatus = navAgent.pathStatus;

        if (navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(2.0f));
            return;
        }
        if ((!hasPath && !pathPending) || pathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestination(true);
        else
            if (navAgent.isPathStale)
            SetNextDestination(false);
    }
    void SetNextDestination(bool increment)
    {
        if (!waypointNetwork) return;
        int incStep = increment ? 1 : 0;
        int nextWaypoint = (currentIndex + incStep >= waypointNetwork.wayPoints.Count) ? 0 : currentIndex + incStep;
        Transform nextWaypointTransform = waypointNetwork.wayPoints[nextWaypoint];
        if (nextWaypointTransform != null)
        {
            currentIndex = nextWaypoint;
            navAgent.destination = nextWaypointTransform.position;
            return;
        }

        currentIndex++;
    }

    IEnumerator Jump(float duration)
    {
        OffMeshLinkData data = navAgent.currentOffMeshLinkData;
        Vector3 startPos = navAgent.transform.position;
        Vector3 endPos = data.endPos + (navAgent.baseOffset * Vector3.up);
        float time = 0f;
        while (time <= duration)
        {
            float t = time / duration;
            navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) + jumpCurve.Evaluate(t) * Vector3.up;
            time += Time.deltaTime;
            yield return null;
        }

        navAgent.CompleteOffMeshLink();
    }
}
