using UnityEngine;

public class moveOnce : MonoBehaviour, ITriggerEvent
{
    private bool activated = false;
    public GameObject destination;
    public Vector3 moveDir;
    public float travelTime;
    private float timePassed;

	public void trigger()
    {
        this.activated = true;
    }

    void Update()
    {
        if (this.activated == true && this.timePassed < this.travelTime)
        {
            this.destination.transform.Translate(this.moveDir * (Time.deltaTime / this.travelTime), Space.World);
            this.timePassed += Time.deltaTime;
        }
    }
}
