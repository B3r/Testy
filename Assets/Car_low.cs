using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_low : MonoBehaviour {
    private Vector3 velocity, input, output;
    private Rigidbody rig;
    private bool isMoving;
    private readonly float DEGREES = 180f;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
        rig.velocity = new Vector3(0, 0, -5);
        isMoving = true;
        output = new Vector4(0.9f, 0.4f, 1f, 0);
        UpdateRotation(output);
        UpdateVelocity(output);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isMoving)
        {
            isMoving = false;
        }
	}

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
