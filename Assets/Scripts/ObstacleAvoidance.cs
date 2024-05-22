using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Steering
{
    private Transform[] Points;

    [Range(0.1f, 10f)]
    [SerializeField] private float radius;

    [Range(0.1f, 20f)]
    [SerializeField] private float maxDist;

    private float StartingMinSeparation = 0;
    private float StartingDistance = 0;
    private float StartingRadius = 0;

    private Transform startingPoint;

    private bool inAgenView = false;

    private void Update()
    {
        RaycastDetectionAvoidance();
    }

    private void RaycastDetectionAvoidance()
    {
        RaycastHit hit;

        //using raycast hitpoints to detect if other AI are infont of the current Agent
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxDist))
        {
            Debug.DrawRay(this.transform.position, this.transform.forward * maxDist, Color.red);

            //if the raycast has hit a collider with the tag agent then inagentveiw bool will become true if not it will become false.
            if (hit.collider.tag == "Agent")
            {
                inAgenView = true;
            }
            else
            {
                inAgenView = false;
            }
        }
    }

    public override SteeringValues GetSteering(Behaviour behaviour)
    {
        SteeringValues steering = new SteeringValues();

        //loweredtime is set equal to float.positiveinfinity as it can involve any value used up to infinity.
        float loweredTime = float.PositiveInfinity;

        //sets the position starting point as nothing
        startingPoint = null;

        //sets both the relatice pos and relative vel at the defualt of zero int X,Y,Z
        Vector3 firstRelativePos = Vector3.zero, firstRelativeVel = Vector3.zero;

        foreach (Transform point in Points)
        {
            //this sets the relative pos = to the distance from this current agent to the stored position in point.
            Vector3 Pos = transform.position - point.position;
            //this gets the velocity from both the point and current AI Agent 
            Vector3 Vel = GetComponent<Rigidbody>().velocity - point.GetComponent<Rigidbody>().velocity;

            //this sets the distance float equal to the magnitude of relative pos giving a numerical distance to use 
            float distance = Pos.magnitude;
            //this does the same as the line above but with the realtive velocity.
            float relativeAcceleration = Vel.magnitude;

            //if the velocity is at zero it will contiue to the next statement after the loop.
            if (relativeAcceleration == 0)
            {
                continue;
            }

            //this slows down the AI from the point by dividing the velocity and distance by it's own magnitude.
            float distanceToCollision = -2 * Vector3.Dot(Pos, Vel) / (relativeAcceleration * relativeAcceleration) / 2;

            // this makes the distance and velocity from the variables and multiples it by the distance to collision to create the sepeartion
            Vector3 separation = Pos + Vel * distanceToCollision;

            //this gives that seperation vector a numrical value asigned to a the minSeparation float.
            float minSeparation = separation.magnitude;

            // this takes the min seperation and checks if it is greater than the set radius with addition to itself so the code will contiue to the next if statement from the for loop.
            if (minSeparation > radius + radius)
            {
                continue;
            }

            /* if the distance to collision is greater than zero and less that the infinity variable known s loweredTime, then the starting variables
               are then set equal to the calcuated variables.*/
            if ((distanceToCollision > 0) && (distanceToCollision < loweredTime))
            {
                loweredTime = distanceToCollision;
                startingPoint = point;
                StartingMinSeparation = minSeparation;
                StartingDistance = distance;
                firstRelativePos = Pos;
                firstRelativeVel = Vel;
                StartingRadius = radius;
            }
        }

        // if the starting point is null return default value back to steering.
        if (startingPoint == null)
        {
            return steering;
        }

        if (StartingMinSeparation <= 0 || StartingDistance < radius + StartingRadius)
        {
            steering.CurrentVelocity = transform.position - startingPoint.position;
        }
        else
        {
            steering.CurrentVelocity = firstRelativePos + firstRelativeVel * loweredTime;
        }

        // this normalize the current velocity so that the maxvelocity can be multiplied onto it.
        steering.CurrentVelocity.Normalize();
        steering.CurrentVelocity *= behaviour.maxVelocity;

        return steering;
    }

    void Start()
    {
        /* the code below finds all the agents with the beaviour component
        on them and asigns thier posions to point aswell as counting the amount of agents */
        Behaviour[] agents = FindObjectsOfType<Behaviour>();
        Points = new Transform[agents.Length - 1];

        int count = 0;

        foreach (Behaviour agent in agents)
        {
            if (agent.gameObject != gameObject)
            {
                Points[count] = agent.transform;
                count++;
            }
        }
    }
}
