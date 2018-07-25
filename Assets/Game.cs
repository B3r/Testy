using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private bool isStopped = true;


	// Use this for initialization
	void Start () {
        isStopped = false;
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
