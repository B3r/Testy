using System;
using UnityEngine;

public class Game : MonoBehaviour {

    private bool isAnyMoving;

    private readonly int carsPerCat = 5;
    private readonly int categoriesCount = 4;
    private float totalTime, roundTime;
    private int frames = 0;

    private readonly string[] categories = { "red", "yellow", "green", "blue" };
    private readonly Color[] colors = { Color.red, Color.yellow, Color.green, Color.blue };
    private readonly Vector3 startingPos = new Vector3(2.5f,0.25f,0);

    private double[][][] bestWeights;

    public Transform car_prefab;
    private Transform[][] cars;
	
    // Use this for initialization
	void Start ()
    {
        totalTime = 0f;
        roundTime = 0f;
        // init Cars
        InitCars();
        isAnyMoving = true;
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
        Debug.Log(roundTime);
        if (!isAnyMoving || roundTime > 60)
        {
            RestartRound();
            return;
        }

        roundTime += Time.deltaTime;
        totalTime += Time.deltaTime;

        frames++;
        if(frames >= 100)
        {
            isAnyMoving = false;
            frames = 0;
            //calculate each second
            for (int x = 0; x < categoriesCount; x++)
            {
                for (int y = 0; y < carsPerCat; y++)
                {
                    Car currentCar = cars[x][y].GetComponent<Car>();
                    if (currentCar.IsMoving)
                    {
                        isAnyMoving = true;
                    }
                }
            }
        }
    }

    private void RestartRound()
    {
        //get best car!
        float bestCar = -1f;
        //stop all cars and reset!
        for (int x = 0; x < categoriesCount; x++)
        {
            for (int y = 0; y < carsPerCat; y++)
            {
                cars[x][y].GetComponent<Rigidbody>().isKinematic = false;
                cars[x][y].GetComponent<Rigidbody>().velocity = Vector3.left;
                cars[x][y].transform.position = startingPos;
                cars[x][y].transform.rotation = Quaternion.identity;
                Car car = cars[x][y].GetComponent<Car>();

                // most successfull car: time travelled * distance travelled
                float currentCarSuccess = car.TimeRunning * car.DistanceTravelled;
                if(currentCarSuccess > bestCar)
                {
                    bestWeights = car.Weights;
                    bestCar = currentCarSuccess;
                }
                car.TimeRunning = 0;
                car.DistanceTravelled = 0;
            }
        }

        // set new weights based on 
        for (int x = 0; x < categoriesCount; x++)
        {
            for (int y = 0; y < carsPerCat; y++)
            {
                Car car = cars[x][y].GetComponent<Car>();
                car.Weights = bestWeights;
            }
        }

        // start round again
        isAnyMoving = true;
        frames = 0;
        roundTime = 0f;
    }
}
