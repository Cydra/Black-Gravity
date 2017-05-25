using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour, IActivateable {

    private bool state = false;
    public Animator anim;
    private ITriggerEvent[] triggers;
    private float animStart = 0f;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        triggers = GetComponents<ITriggerEvent>();
    }

    public void activate()
    {
        if ((Time.time - animStart) >= 1.75f)                        // make sure last animation was finished
        {
            playAnimation();
            foreach (ITriggerEvent trigger in triggers)             // activates trigger function of each component that implements ITriggerEvent
            {
                trigger.trigger();
            }
        }
    }

    void playAnimation()                                        // Lever Animation
    {
        animStart = Time.time;
        if (!state)
        {
            anim.Play("Lever_forward", -1, 0f);
            state = true;
        }
        else
        {
            anim.Play("Lever_backwards", -1, 0f);
            state = false;
        }
}
}