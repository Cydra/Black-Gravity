using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    z, y, x
}

public class RotateObject : MonoBehaviour, ITriggerEvent
{

    public float angle = 0f;
    public float time = 0;
    public bool back = false;
    public Axis rotateAxis = Axis.z;
    public GameObject destination;
    private float timePassed = 0f;
    private bool triggered = false;

    void Start()
    {
        if (this.time < 0f) this.time = 0;
        this.angle *= -1;
        this.timePassed = this.time;
    }

    public void trigger()
    {
        this.triggered = true;
        this.angle *= -1;                                                                                                                // change Direction
        if (this.timePassed < this.time) this.timePassed = this.time - this.timePassed;                                                                      // set timePassed so that if triggered midway again the roation wont go further than the original position
        else this.timePassed = 0f;
    }

    void Update ()
    {
        if (this.triggered == true)
        {
            if (this.timePassed < this.time)
            {
                switch (this.rotateAxis)
                {
                    case Axis.z:
                        {
                            this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.forward, (this.angle * Time.deltaTime) / this.time);
                            break;
                        }
                    case Axis.y:
                        {
                            this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.up, (this.angle * Time.deltaTime) / this.time);
                            break;
                        }
                    case Axis.x:
                        {
                            this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.right, (this.angle * Time.deltaTime) / this.time);
                            break;
                        }
                }

                this.timePassed += Time.deltaTime;
            }
            else if (this.back == true)
            {
                this.angle *= -1;
                this.back = false;
                this.timePassed = 0;
            }
        }
    }
}
