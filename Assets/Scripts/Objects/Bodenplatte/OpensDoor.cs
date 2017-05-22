using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpensDoor : MonoBehaviour, ITriggerEvent
{

    public GameObject door;

	public void trigger()
    {
        if (door.activeSelf == false) door.SetActive(true);
        else door.SetActive(false);
    }
}
