using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IActivateable {

    private bool state = false;
    private Animator anim;
    private ITriggerEvent[] triggers;
    private float animStart = 0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        triggers = GetComponents<ITriggerEvent>();
    }

    public void activate()
    {
        if ((Time.time - animStart) >= 1f)                        // make sure last animation was finished
        {
            print("playi anim?");
            playAnimation();
            foreach (ITriggerEvent trigger in triggers)             // activates trigger function of each component that implements ITriggerEvent
            {
                trigger.trigger();
            }
        } else print("what is dis shit?");
    }

    void playAnimation()                                        // Lever Animation
    {
        print("should play");
        animStart = Time.time;
        anim.Play("Click", -1, 0f);
    }
}
