using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    GameManager gameManager;

    float mutationRate = 0.05f; //Chance that any directions gets changed

    [HideInInspector]
    public Vector3[] directions;
    [HideInInspector]
    public int directionsSize;
    [HideInInspector]
    public int step = 0;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        directionsSize = 500;
        randomizeDirections();
    }

    public void randomizeDirections()  //Set random directions
    {
        if (gameManager.population1.generation == 1)  //If the array is empty
        {
            directions = new Vector3[directionsSize];
            for (int i = 0; i < directionsSize; i++)
            {
                float randomAngle = Random.Range(0, 2 * Mathf.PI);
                directions[i] = new Vector3(Mathf.Cos(randomAngle), 1, Mathf.Sin(randomAngle));
            }
        }
    }

    /*public Brain clone()
    {
        this.step = 0;
        return this;
    }*/
    
    /*public void mutate()
    {
        Debug.Log("MUTATE!");
        for (int i = 0; i < directions.Length; i++)
        {
            float random = Random.Range(0f,1f);

            if (random < mutationRate)
            {                
                //Set direction to a new random
                float randomAngle = Random.Range(0, 360);

                directions[i] =  new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad),1 ,Mathf.Sin(randomAngle * Mathf.Deg2Rad));
            }
        }
    }*/
}
