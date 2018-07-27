using System;
using UnityEngine;

public class Game : MonoBehaviour {

    private bool isAnyMoving;

    private readonly int carsPerCat = 5;
    private readonly int categoriesCount = 4;
    private readonly int transitions = 3;
    private readonly int[] neuronsPerLayers = {3,5,4,4};
    private int frames = 0;
  
        
    private string[] categories = { "red", "yellow", "green", "blue" };
    private Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue };
    private double[][][][] globalWeights = new double[4][][][]; //weights per category
    private readonly Vector3 startingPos = new Vector3(2.5f,0.25f,0);

    public Transform car_prefab;
    private Transform[][] cars;
	
    // Use this for initialization
	void Start ()
    {
        // init Cars
        InitWeights();
        InitCars();
        isAnyMoving = true;
    }

    private void InitWeights()
    {
        for (int x = 0; x < globalWeights.Length; x++) // for each category
        {
            globalWeights[x] = new double[transitions][][];
            for(int y = 0; y < transitions; y++) // for each transition
            {
                globalWeights[x][y] = new double[neuronsPerLayers[y+1]][];
                for (int z = 0; z < globalWeights[x][y].Length; z++) // for each outputNeuron
                {
                    globalWeights[x][y][z] = new double[neuronsPerLayers[y]]; //set an inputNeuron Vector


                    // TODO: fill initial weights
                }
            }
        }
    }

    private void InitCars()
    {
        cars = new Transform[categoriesCount][];
        for (int x = 0; x < categoriesCount; x++)
        {
            cars[x] = new Transform[carsPerCat];
            for (int y = 0; y < carsPerCat; y++)
            {
                Transform car = Instantiate(car_prefab, startingPos, Quaternion.identity);
                car.name = categories[x] + "car" + y;
                car.GetComponent<Renderer>().material.color = colors[x];
                cars[x][y] = car;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isAnyMoving)
        {
            RestartRound();
            return;
        }

        frames++;
        if(frames >= 100)
        {
            frames = 0;
        
            //calculate each second
            for (int x = 0; x < categoriesCount; x++)
            {
                for (int y = 0; y < carsPerCat; y++)
                {
                    Vector4 sampleOutput = CalculateOutputNeuronsForCar(cars[x][y]);
                
                    Car currentCar = cars[x][y].GetComponent<Car>();
                    
                    if (currentCar.IsMoving)
                    {
                        isAnyMoving = true;
                        currentCar.UpdateRotation(sampleOutput);
                        currentCar.UpdateVelocity(sampleOutput);
                    }
                }
            }
        }
    }

    private Vector4 CalculateOutputNeuronsForCar(Transform transform)
    {
        Car car = transform.gameObject.GetComponent<Car>();
        double[] inputNeurons = {car.DistLeft,car.DistRight,car.CurrentSpeed};
        
        double[][] weights; //TODO calc weights by strategy
        double[] outputNeurons = Sigmoid(weights, inputNeurons);
    }

    private double[] Sigmoid(double[][] weights, double[] inputNeurons)
    {
        double[] outputNeurons = new double[weights.Length];
        double neuronValue;
        for(int i = 0; i < weights.Length; i++) // for each layer
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

    private void RestartRound()
    {

        //destroy cars
        for (int x = 0; x < categoriesCount; x++)
        {
            for (int y = 0; y < carsPerCat; y++)
            {
                Destroy(cars[x][y].gameObject);
            }
        }

        //create new cars
        InitCars();

        // start round again
        isAnyMoving = true;
        frames = 0;
    }
}
