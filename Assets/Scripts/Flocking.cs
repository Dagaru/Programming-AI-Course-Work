using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : Steering
{
    //this strores the positions as an array.
    private Transform[] Points;

    public int nearAgentCount = 0;

    [Range(0.1f, 10f)]
    [SerializeField] private float Radius = 2f;

    [Range(-100f, 0)]
    [SerializeField] private float decayCoefficient = -25f;

    //this calls upon the seperation method once game begins and code is active
    private void Start()
    {
        Seperation();
    }

    private void Seperation()
    {
        // this stores any agent that has the behaviour class on them
        Behaviour[] AIAgents = FindObjectsOfType<Behaviour>();

        //this stores each ai agent positions
        Points = new Transform[AIAgents.Length - 1];

        foreach (Behaviour agent in AIAgents)
        {
            if (agent.gameObject != gameObject)
            {
                //if there is any agent in the scene that is not itself it will get that's agents position.
                Points[nearAgentCount] = agent.transform;
                // using the same method as the above line it also counts that other object/AIAgent as 1 near agent 
                nearAgentCount++;
            }
        }
    }

    // this overive is used to send the data back from this class to the steering class and even use or change data from the behaviour class.
    public override SteeringValues GetSteering(Behaviour behaviour)
    {
        SteeringValues steering = new SteeringValues();

        foreach(Transform point in Points)
        {
            // this finds the direction from current object to other agent objects
            Vector3 direction = point.transform.position - transform.position;

            // this creates a between the value of the line above.
            float distance = direction.magnitude;

            // if the distance between the two are below the radius. it then creates the computation stop the AI at the set radius from each other.
            if(distance < Radius)
            {
                float stregnth = Mathf.Min(decayCoefficient / (distance * distance), behaviour.maxVelocity);

                direction.Normalize();

                steering.CurrentVelocity += stregnth * direction;
            }
        }

        //this returns the computation from above.
        return steering;
    }
}
