using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    back, loop
}

public class moveObject : MonoBehaviour, ITriggerEvent
{
    public GameObject WaypointParent;
    public GameObject destination;
    public float time;
    public Mode mode = Mode.back;

    private List<Vector3> waypoints = new List<Vector3>(); 
    private bool triggered;
    private int nextWaypoint = 0;
    private bool backwards = false;
    private float timePassed = 0f;
    private Vector3 dir;

    public void trigger()
    {
        triggered = true;
    }

    
	void Start () {
        if (time < 0) time = 0;
        print(WaypointParent.transform.childCount);
        for (int i = 0; i < WaypointParent.transform.childCount; ++i)
        {
            waypoints.Add(WaypointParent.transform.GetChild(i).position);
        }

        //foreach (Transform child in WaypointParent.transform)
        //{
        //    waypoints.Add(child.position);
        //    print("Child found");
        //}

        dir = nextDir();
	}
	
	void Update () {
        if (triggered == true)
        {
            if (timePassed < time)
            {
                print(dir);
                destination.transform.Translate(dir * (Time.deltaTime / time), Space.World);
                timePassed += Time.deltaTime;
            }
            else
            {
                dir = nextDir();
                timePassed = 0f;
            }
        }
	}


    private Vector3 nextDir()
    {
        print("getting next point");
        if (nextWaypoint - 1 >= waypoints.Count)
        {
            if (mode == Mode.back)
            {
                backwards = true;
            }
            else
            {
                nextWaypoint = 0;
            }
        }
        else if (nextWaypoint == 0 && backwards == true)
        {
            backwards = false;
        }

        Vector3 dir;
        if (backwards == false)
        {
            dir = waypoints[nextWaypoint] - destination.transform.position;
            nextWaypoint++;
        }
        else
        {
            dir = waypoints[nextWaypoint] - destination.transform.position;
            nextWaypoint--;
        }

        return dir;
    }
}
