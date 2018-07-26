using UnityEngine;

public class Game : MonoBehaviour {

    private bool isStopped = true;
    private readonly int carsPerCat = 5;
    private readonly int categoriesCount = 4;
    private string[] categories = new string[4];
    private readonly Vector3 startingPos = new Vector3(2.5f,0.25f,0);
    public Transform car_prefab;
    private Transform[][] cars;
	// Use this for initialization
	void Start ()
    {
        isStopped = true;
        categories[0] = "red";
        categories[1] = "yellow";
        categories[2] = "green";
        categories[3] = "blue";

        // init Cars
        initCars(categories);
    }

    private void initCars(string[] categories)
    {
        cars = new Transform[categoriesCount][];
        for (int x = 0; x < categoriesCount; x++)
        {
            cars[x] = new Transform[carsPerCat];
            for (int y = 0; y < carsPerCat; y++)
            {
                Transform car = cars[0][y] = Instantiate(car_prefab, startingPos, Quaternion.identity);
                car.name = categories[x] + "car" + y;
                // color here
                cars[0][y] = car;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        
        if (isStopped)
        {
            // if all cars stopped, restart round
            RestartRound();
            return;
        }

        //calculate each second
	}

    private void RestartRound()
    {
        //destroy cars
        //create new cars
        
        isStopped = false; // start round again
    }
}
