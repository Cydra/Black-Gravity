using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnCol : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        ITriggerEvent[] events =  this.GetComponents<ITriggerEvent>();
        foreach(ITriggerEvent myEvent in events)
        {
            myEvent.trigger();
        }
    }
}
