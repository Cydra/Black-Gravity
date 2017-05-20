using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for activateable objects' events, create one child for each event
public interface ITriggerEvent {

    void trigger();
}
