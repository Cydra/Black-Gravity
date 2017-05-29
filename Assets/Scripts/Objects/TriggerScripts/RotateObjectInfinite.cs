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
        if (this.time < 0f) this.time = 0;
    }

    public void trigger()
    {
        if (this.active == false) this.active = true;
        else this.active = false;
    }

    void Update()
    {
        if (this.active == true)
        {
            float tmpAngle = (this.angle * Time.deltaTime) / this.time;
            this.totalAngle += tmpAngle;
            switch (this.rotateAxis)
            {
                case Axis.z:
                    {
                        this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.forward, tmpAngle);
                        break;
                    }
                case Axis.y:
                    {
                        this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.up, tmpAngle);
                        break;
                    }
                case Axis.x:
                    {
                        this.destination.transform.RotateAround(this.destination.transform.position, this.destination.transform.right, tmpAngle);
                        break;
                    }
            }

            if (this.back == true && Mathf.Abs(this.totalAngle) >= Mathf.Abs(this.angle))
            {
                this.totalAngle -= this.angle;
                this.angle *= -1;
            }
        }
    }
}
