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
        if (time < 0f) time = 0;
        angle *= -1;
        timePassed = time;
    }

    public void trigger()
    {
        triggered = true;
        angle *= -1;                                                                                                                // change Direction
        if (timePassed < time) timePassed = time - timePassed;                                                                      // set timePassed so that if triggered midway again the roation wont go further than the original position
        else timePassed = 0f;
    }

    void Update ()
    {
        if (triggered == true)
        {
            if (timePassed < time)
            {
                switch (rotateAxis)
                {
                    case Axis.z:
                        {
                            destination.transform.RotateAround(destination.transform.position, destination.transform.forward, (angle * Time.deltaTime) / time);
                            break;
                        }
                    case Axis.y:
                        {
                            destination.transform.RotateAround(destination.transform.position, destination.transform.up, (angle * Time.deltaTime) / time);
                            break;
                        }
                    case Axis.x:
                        {
                            destination.transform.RotateAround(destination.transform.position, destination.transform.right, (angle * Time.deltaTime) / time);
                            break;
                        }
                }

                timePassed += Time.deltaTime;
            }
            else if (back == true)
            {
                angle *= -1;
                back = false;
                timePassed = 0;
            }
        }
    }
}
