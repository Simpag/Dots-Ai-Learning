using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int populationSize;

    public Population population1;
    public GameObject dotPrefab;

	// Use this for initialization
	void Start () {
        population1 = new Population(populationSize, dotPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		if (population1.allDotsDead())
        {
            Debug.Log("NEW POPULATION!");
            population1.calculateFitness();
            population1.naturalSelection(); //New population
        }
	}
}

/*
 *skapa array med game objekt 
 * skapar array med dots
 * följ exempelkoden
 * attacha dotsen till gameobjects
 * ta bort dotsen från gameobjekten
 * skapa nya dots
 * repeat
 */
