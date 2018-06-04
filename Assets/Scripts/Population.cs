using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour {
    public int generation = 1;

    int populationSize;
    GameObject prefab;
    Material bestDotMaterial, defaultDotMaterial;

    float fitnessSum;

    int bestDot = 0;

    public int minStep = 1000;

    public GameObject[] population;

    public Population(int pop, GameObject _prefab)
    {
        populationSize = pop;
        prefab = _prefab;

        population = new GameObject[populationSize];
        InstantiateDots(population);
    }

    void InstantiateDots(GameObject[] _pop)
    {
        for (int i = 0; i < populationSize; i++)
        {
            //Debug.Log("Instatiate");
            _pop[i] = Instantiate(prefab);    //Instantiate objects
            _pop[i].name = "Dot " + i;    //Rename object in order
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < populationSize; i++)
        {
            Dot _dot = population[i].GetComponent<Dot>();
            if (_dot.brain.step > minStep)
            {//if the dot has already taken more steps than the best dot has taken to reach the goal
                _dot.dead = true;//then it dead
            }
        }
    }

    public void calculateFitness()
    {
        for (int i = 0; i < populationSize; i++)
        {
            population[i].GetComponent<Dot>().calculateFitness();
        }
    }

    public bool allDotsDead()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Dot _dot = population[i].GetComponent<Dot>();
            if (!_dot.dead && !_dot.reachedGoal)
                return false;
        }

        //if it reached the goal or is dead
        return true;
    }

    void setBestDotIndex()
    {
        float max = 0;
        int maxIndex = 0;
        for (int i = 0; i < populationSize; i++)
        {
            Dot _dot = population[i].GetComponent<Dot>();
            if (_dot.fitness > max)
            {
                //Debug.Log(population[i].fitness + " > " + max);
                max = _dot.fitness;
                maxIndex = i;
            }
        }

        bestDot = maxIndex;

        //if the best dot reached the goal then reset the minimum number of steps it takes to get to the goal
        Dot _bestDot = population[bestDot].GetComponent<Dot>();
        if (_bestDot.reachedGoal)
        {
            minStep = _bestDot.brain.step;
            Debug.Log("Best step:" + minStep);
        }
    }

    void calculateFitnessSum()
    {
        fitnessSum = 0;
        for (int i = 0; i < populationSize; i++)
        {
            Dot _dot = population[i].GetComponent<Dot>();
            fitnessSum += _dot.fitness;
        }
    }

    GameObject selectParent(GameObject[] _pop)
    {
        float randomFitness = Random.Range(0, fitnessSum);
        float runningSum = 0;

        for (int i = 0; i < populationSize; i++)
        {
            runningSum += _pop[i].GetComponent<Dot>().fitness;
            if (runningSum > randomFitness)
            {
                return _pop[i];
            }
        }

        //Shouldnt get to here
        Debug.Log("No PARENT!");
        return null;
    }

    public void naturalSelection()
    {
        Vector3[][] _listOfDir = new Vector3[populationSize][];

        //Reset best dot
        Dot _bestDot = population[bestDot].GetComponent<Dot>();
        _bestDot.isBest = false;

        //Calculate best dot and fitness sum
        setBestDotIndex();
        calculateFitnessSum();

        //New dot population
        GameObject[] newPopulation = new GameObject [populationSize];
        InstantiateDots(newPopulation);

        //Set the best dot
        _bestDot = population[bestDot].GetComponent<Dot>();
        
        //Debug.Log("Best dot: " + population[bestDot].name);
        //Debug.Log("First direction of best dot: " + _bestDot.brain.directions[0].ToString() + " Second: " + _bestDot.brain.directions[1]);

        //Assign the best dot to place 0
        newPopulation[0].GetComponent<Dot>().isBest = true;
        _listOfDir[0] = _bestDot.brain.directions;

        //Copy directions of previous dots for the new brains
        for (int i = 1; i < populationSize; i++)
        {
            //Select a parent for the dot
            GameObject parent = selectParent(population);

            //Copy directions
            _listOfDir[i] = parent.GetComponent<Brain>().directions;

            //Mutate some directions
            
        }

        //Debug.Log("Original best: " + _bestDot.brain.directions[0] + " Copy best: " + newPopulation[0].GetComponent<Dot>().brain.directions[0]);

        //Destroy population game objects
        //And add the new dots to the population
        for (int i = 0; i < populationSize; i++)
        {
            //Destroy the object
            Destroy(population[i]);

            //Add reference to new object
            population[i] = newPopulation[i];

            //Add the directions
            population[i].GetComponent<Brain>().directions = _listOfDir[i];
        }

        //Debug.Log("New best: " + population[0].GetComponent<Dot>().brain.directions[0]);

        generation++;
    }

    public void mutateDirection()
    {
        
    }

    /*public void mutateDots()
    {
        //Mutate every dot except the best dot
        for (int i = 1; i < populationSize; i++)
        {
            population[i].GetComponent<Dot>().mutate();
        }
    }*/
}
