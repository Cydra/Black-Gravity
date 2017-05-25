using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectInfinite : MonoBehaviour, ITriggerEvent {

    public float angle = 0f;
    public float time = 0;
    public bool back = false;
    public Axis rotateAxis = Axis.z;
    public GameObject destination;
    private bool active = false;
    private float totalAngle = 0f;

    void Start()
    {
        if (time < 0f) time = 0;
    }

    public void trigger()
    {
        if (active == false) active = true;
        else active = false;
    }

    void Update()
    {
        if (active == true)
        {
            float tmpAngle = (angle * Time.deltaTime) / time;
            totalAngle += tmpAngle;
            switch (rotateAxis)
            {
                case Axis.z:
                    {
                        destination.transform.RotateAround(destination.transform.position, destination.transform.forward, tmpAngle);
                        break;
                    }
                case Axis.y:
                    {
                        destination.transform.RotateAround(destination.transform.position, destination.transform.up, tmpAngle);
                        break;
                    }
                case Axis.x:
                    {
                        destination.transform.RotateAround(destination.transform.position, destination.transform.right, tmpAngle);
                        break;
                    }
            }

            if (back == true && Mathf.Abs(totalAngle) >= Mathf.Abs(angle))
            {
                totalAngle -= angle;
                angle *= -1;
            }
        }
    }
}
