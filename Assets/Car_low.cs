using UnityEngine;

public class Car_low : MonoBehaviour {
    private Vector3 velocity, input, output;
    private Rigidbody rig;
    private bool isMoving;
    private readonly float DEGREES = 180f;
    private float distLeft, distRight, currentSpeed;
    private Vector3[] weights;

    public Vector3[] Weights
    {
        get
        {
            return weights;
        }

        set
        {
            weights = value;
        }
    }

    // Use this for initialization
    void Start () {
        weights = new Vector3[5];
        rig = GetComponent<Rigidbody>();
        rig.velocity = new Vector3(0, 0, -5f);
        isMoving = true;
        output = new Vector4(0.9f, 0.4f, 1f, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isMoving)
        {
            RaycastHit hitRight, hitLeft;

            //get distance to right
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitRight, Mathf.Infinity))
            {
                distRight = hitRight.distance;
            }
            //get distance to left
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitLeft, Mathf.Infinity))
            {
                distLeft = hitLeft.distance;
            }
            //get speed
            float currentSpeed = rig.velocity.magnitude;

            // neuronal here
            //output = calcOutputNeurons(distLeft,distRight,currentSpeed);
            //UpdateRotation(output);
            //UpdateVelocity(output);
        }
	}

    //turn left or right based on 180 degrees
    void UpdateRotation(Vector4 output)
    {
        float angle = (output[0] - output[1]) * DEGREES;
        transform.Rotate(Vector3.up, angle);
        Debug.Log("Rotation: " + rig.rotation);
    }

    void UpdateVelocity(Vector4 output)
    {
        Vector3 vel = rig.velocity;
        float deltaSpeed = output[2] - output[3];
        float magn = vel.magnitude;
        float angle = rig.rotation.y * DEGREES;
        float resultX = (magn + deltaSpeed) * Mathf.Sin(angle);
        float resultZ = (magn + deltaSpeed) * Mathf.Cos(angle);
        rig.velocity = new Vector3(resultX, 0, resultZ) * Time.deltaTime;
        Debug.Log("Velocity: " + rig.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "left" || collision.gameObject.name == "right" || collision.gameObject.name == "top" || collision.gameObject.name == "bot" || collision.gameObject.name == "mid")
        {
            Stop();
            isMoving = false;
        }
    }

    void Stop()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.isKinematic = true;
        rig.Sleep();
        Debug.Log("Position is: " + rig.position);
    }

}
