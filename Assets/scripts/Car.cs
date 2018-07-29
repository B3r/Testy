using System;
using UnityEngine;

public class Car : MonoBehaviour {
    private Rigidbody rig;
    private readonly float DEGREES = 90f;
    private Vector3 lastPosition;

    private readonly int transitions = 3;
    private readonly int[] neuronsPerLayers = { 3, 5, 4, 4 };

    public float DistLeft { get; set; }

    public float DistRight { get; set; }

    public float CurrentSpeed { get; set; }

    public bool IsMoving { get; set; }

    public double[][][] Weights { get; set; }

    public float TimeRunning { get; set; }

    public float DistanceTravelled { get; set; }

    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
        rig.velocity = Vector3.left;
        InitWeights();
        TimeRunning = 0;
        DistanceTravelled = 0;
        lastPosition = transform.position;
        IsMoving = true;
    }

    private void InitWeights()
    {
        Weights = new double[transitions][][];
        for (int x = 0; x < transitions; x++) // for each transition
        {
            Weights[x] = new double[neuronsPerLayers[x + 1]][];
            for (int y = 0; y < Weights[x].Length; y++) // for each outputNeuron
            {
                Weights[x][y] = new double[neuronsPerLayers[x]]; //set an inputNeuron Vector
                for (int z = 0; z < Weights[x][y].Length; z++)
                {
                    // fill initial weights with ones
                    Weights[x][y][z] = UnityEngine.Random.Range(0.0f, 1.0f);
                }
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (IsMoving)
        {
            DistanceTravelled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
            TimeRunning += Time.deltaTime;
            RaycastHit hitRight, hitLeft;

            //get distance to right
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(transform.forward), out hitRight, Mathf.Infinity))
            {
                DistRight = hitRight.distance;
            }
            //get distance to left
            if (Physics.Raycast(transform.position, transform.TransformDirection(transform.forward), out hitLeft, Mathf.Infinity))
            {
                DistLeft = hitLeft.distance;
            }
            //get speed
            CurrentSpeed = rig.velocity.magnitude;
            if(CurrentSpeed == 0)
            {
                IsMoving = false;
                return;
            }
            double[] inputVector = {DistLeft,DistRight,CurrentSpeed};
            double[] outputVector = {0,0,0,0};
            // call sigmoid for each transition
            for(int i = 0; i < 3; i++)
            {
                outputVector = Sigmoid(Weights[i],inputVector);
                inputVector = outputVector;
            }
            UpdateRotation(outputVector);
            UpdateVelocity(outputVector);
        }
	}

    //turn left or right based on 180 degrees
    public void UpdateRotation(double[] output)
    {
        float angle = (float)(output[0] - output[1]) * DEGREES;
        // Rotate around y - axis
        transform.Rotate(Vector3.up, angle);
    }

    public void UpdateVelocity(double[] output)
    {
        float deltaSpeed = (float)(output[2] - output[3]) * Time.deltaTime;
        CharacterController controller = GetComponent<CharacterController>();

        // Move forward / backward
        controller.SimpleMove(transform.forward * deltaSpeed);

        //float angle = rig.rotation.y * DEGREES;
        //float resultX = (CurrentSpeed + deltaSpeed) * Mathf.Sin(angle);
        //float resultZ = (CurrentSpeed + deltaSpeed) * Mathf.Cos(angle);
        //rig.velocity = new Vector3(resultX, 0, resultZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "left" || collision.gameObject.name == "right" || collision.gameObject.name == "top" || collision.gameObject.name == "bot" || collision.gameObject.name == "mid")
        {
            Stop();
            IsMoving = false;
            Debug.Log("Car collided!!!");
        }
    }

    void Stop()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.isKinematic = true;
    }

    private double[] Sigmoid(double[][] weights, double[] inputNeurons)
    {
        double[] outputNeurons = new double[weights.Length];
        double neuronValue;
        for (int i = 0; i < weights.Length; i++) // for each layer
        {
            neuronValue = 0f;
            for (int j = 0; j < weights[i].Length; j++) // for each input
            {
                neuronValue += weights[i][j] * inputNeurons[j];
            }
            outputNeurons[i] = 1 / (1 + Math.Exp(neuronValue));
        }
        return outputNeurons;
    }
}
