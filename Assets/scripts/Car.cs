using UnityEngine;

public class Car : MonoBehaviour {
    private Rigidbody rig;
    private readonly float DEGREES = 180f;

    public float DistLeft { get; set; }

    public float DistRight { get; set; }

    public float CurrentSpeed { get; set; }

    public bool IsMoving { get; set; }

    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
        rig.velocity = new Vector3(0f, 0f, 0f);
        IsMoving = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (IsMoving)
        {
            RaycastHit hitRight, hitLeft;

            //get distance to right
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitRight, Mathf.Infinity))
            {
                DistRight = hitRight.distance;
            }
            //get distance to left
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitLeft, Mathf.Infinity))
            {
                DistLeft = hitLeft.distance;
            }
            //get speed
            CurrentSpeed = rig.velocity.magnitude;
            if(CurrentSpeed == 0)
            {
                IsMoving = false;
            }
        }
	}

    //turn left or right based on 180 degrees
    public void UpdateRotation(Vector4 output)
    {
        float angle = (output[0] - output[1]) * DEGREES;
        transform.Rotate(Vector3.up, angle);
    }

    public void UpdateVelocity(Vector4 output)
    {
        float deltaSpeed = (output[2] - output[3]) * Time.deltaTime;
        float angle = rig.rotation.y * DEGREES;
        float resultX = (CurrentSpeed + deltaSpeed) * Mathf.Sin(angle);
        float resultZ = (CurrentSpeed + deltaSpeed) * Mathf.Cos(angle);
        rig.velocity = new Vector3(resultX, 0, resultZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "left" || collision.gameObject.name == "right" || collision.gameObject.name == "top" || collision.gameObject.name == "bot" || collision.gameObject.name == "mid")
        {
            Stop();
            IsMoving = false;
        }
    }

    void Stop()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.isKinematic = true;
        rig.Sleep();
    }

}
