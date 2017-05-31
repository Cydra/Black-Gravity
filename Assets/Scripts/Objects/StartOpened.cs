using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOpened : MonoBehaviour {

    private List<LineRenderer> lasers = new List<LineRenderer>();
    Collider collider;
    // Use this for initialization
    void Start()
    {
        lasers.Add(this.transform.Find("Laser_1").gameObject.GetComponent<LineRenderer>());
        lasers.Add(this.transform.Find("Laser_2").gameObject.GetComponent<LineRenderer>());
        lasers.Add(this.transform.Find("Laser_3").gameObject.GetComponent<LineRenderer>());
        lasers.Add(this.transform.Find("Laser_4").gameObject.GetComponent<LineRenderer>());

        collider = this.GetComponent<BoxCollider>();

        foreach (LineRenderer laser in this.lasers)
        {
            laser.enabled = false;
        }
        this.collider.enabled = false;
    }
}
