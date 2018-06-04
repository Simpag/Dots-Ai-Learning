using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

    public Brain brain;
    public Material bestMaterial;

    Vector3 startPosition = new Vector3(0f, 1f, -50f);
    Vector3 goal;

    Vector3 Position;
    Vector3 Velocity;
    Vector3 Acceleration;

    float maxSpeed = .1f;
    float speed = 0.05f;

    public bool dead = false;
    public bool reachedGoal = false;
    public bool isBest = false;

    public float fitness = 0;

    private void Start()
    {
        goal = GameObject.Find("Goal").transform.position;

        if (isBest)
        {
            this.GetComponent<MeshRenderer>().material = bestMaterial;
        }

        Velocity.Set(0, 0, 0);
        Acceleration.Set(0, 0, 0);

        //Set start position of dot.
        Debug.Log("Set default position");
        this.transform.position = startPosition;
        Position = startPosition;
    }

    void move()
    {
        if (brain.directionsSize > brain.step)  //If there's still directions left in the array
        {
            Acceleration = brain.directions[brain.step];
            brain.step++;
            //Debug.Log("Step++");
        }
        else
        {
            dead = true;
            //Debug.Log("Step: " + brain.step + " Max step: " + brain.directionsSize);
            Debug.Log("DIED! TOO MANY STEPS!");
        }

        //Apply the acceleration and move the dot
        Velocity.x = Velocity.x + Acceleration.x * speed;
        Velocity.z = Velocity.z + Acceleration.z * speed;
        Mathf.Clamp(Velocity.x, -maxSpeed, maxSpeed);
        Mathf.Clamp(Velocity.z, -maxSpeed, maxSpeed);
        Position.x += Velocity.x;
        Position.z += Velocity.z;

        this.transform.position = Position;
    }

    private void Update()
    {
        brain = GetComponent<Brain>();

        //Debug.Log("Dead: " + dead + " Goal: " + reachedGoal);
        if (!dead && !reachedGoal)
        {
            move();
            //If the dot reached the goal
            if (this.transform.position.x > 64 || this.transform.position.x < -64 || this.transform.position.z > 64 || this.transform.position.z < -64)
            {
                Debug.Log("OUT OF BOUNDS! DIED!");
                dead = true; //If it's fallen off the platform
            }
            else if (Vector3.Distance(goal, this.transform.position) < 5 )
            {
                reachedGoal = true;
            }
        }
    }

    public void calculateFitness()
    {
        brain = GetComponent<Brain>();
        //Debug.Log(this.name + " Steps: " + brain.step);
        if (reachedGoal)    // if it reached the goal then fitness is based on the number of steps it took to get there
        {
            fitness = 1f / 16f + 10000f / (Mathf.Pow((float)brain.step, 2f));
        }
        else // if it didnt reach the goal then the fitness is based on how close it got to the goal
        {
            fitness = 1f / Mathf.Pow(Vector3.Distance(goal, this.transform.position), 2);
        }
    }

    /*public Brain getBrainClone()
    {
        Brain _copy = new Brain();
        _copy = this.brain;

        return _copy;
    }*/

    /*public void resetSteps()
    {
        this.brain.step = 0;
    }*/

    /*public void mutate()
    {
        if (!isBest)
        {
            GetComponent<Brain>().mutate();
        }
    }*/
}
