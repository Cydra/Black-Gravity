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
        if (this.triggered == false) this.triggered = true;
        else this.triggered = false;
    }

    
	void Start () {
        if (time < 0) time = 0;
        for (int i = 0; i < this.WaypointParent.transform.childCount; ++i)
        {
            this.waypoints.Add(this.WaypointParent.transform.GetChild(i).position);
        }

        this.dir = this.nextDir();
	}
	
	void Update () {
        if (this.triggered == true)
        {
            if (this.timePassed < this.time)
            {
                this.destination.transform.Translate(this.dir * (Time.deltaTime / this.time), Space.World);
                this.timePassed += Time.deltaTime;
            }
            else
            {
                this.dir = this.nextDir();
                this.timePassed = 0f;
            }
        }
	}


    private Vector3 nextDir()
    {
        if (this.nextWaypoint > this.waypoints.Count - 1)
        {
            if (mode == Mode.back)
            {
                this.backwards = true;
                this.nextWaypoint -= 2;
            }
            else
            {
                this.nextWaypoint = 0;
            }
        }
        else if (this.nextWaypoint == 0 && this.backwards == true)
        {
            this.backwards = false;
        }

        Vector3 dir;
        if (this.backwards == false)
        {
            dir = this.waypoints[this.nextWaypoint] - this.destination.transform.position;
            this.nextWaypoint++;
        }
        else
        {
            dir = this.waypoints[this.nextWaypoint] - this.destination.transform.position;
            this.nextWaypoint--;
        }

        return dir;
    }
}
