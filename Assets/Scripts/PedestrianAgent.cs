using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PedestrianAgent : Steering
{
    [SerializeField] private GameObject Target1;
    [SerializeField] private GameObject Target2;

    private bool SwitchTarget = false;

    private float DistanceTargetA;
    private float DistanceTargetB;

    [Range(0.1f, 20f)]
    [SerializeField] private float SwitchRadius = 6f;

    private void Start()
    {
        // to make sure that swithc taget is false so that the AI Agents go to thier default starting target.
        SwitchTarget = false;
    }

    void Update()
    {
        CheckIfAtTarget();
    }
    
    public override SteeringValues GetSteering(Behaviour behaviour)
    { 
        SteeringValues steering = new SteeringValues();
        /* if Switch Target is true This seeks out target 1 at the set speed of the maxVelocity,
           it also looks towards the target it is trying to reach */
        if(SwitchTarget == true)
        {
          this.transform.LookAt(Target1.transform.position);
          steering.CurrentVelocity = Target1.transform.position - this.transform.position; 
          //steering.angular = 0;
        }
        else
        {
          /* if Switch Target is false This seeks out target 1 at the set speed of the maxVelocity,
             it also looks towards the target it is trying to reach */
          this.transform.LookAt(Target2.transform.position);
          steering.CurrentVelocity = Target2.transform.position - this.transform.position;
          //steering.angular = 0;
        }

        // This Normalizes the current velocity so it can be multiplied by the maxVelocity which is the AIAgents speed.
        steering.CurrentVelocity.Normalize();
        steering.CurrentVelocity *= behaviour.maxVelocity;
        return steering;
    }

    private void CheckIfAtTarget()
    {
        // these two vector3's below take store the distance from the targets to this objects position
        Vector3 A = Target2.transform.position - this.transform.position;
        Vector3 B = Target1.transform.position - this.transform.position;

        //These two floats below take in the magnatude of the above vector3's so the distance between the two can be storeds into a floating point.
        DistanceTargetA = A.magnitude;
        DistanceTargetB = B.magnitude;

        // these if statments check the distance of this object to the targets and switch target changes depending on which object it is closest too.
        if (DistanceTargetA <= SwitchRadius)
        {
            SwitchTarget = true;
        }

        if (DistanceTargetB <= SwitchRadius)
        {
            SwitchTarget = false;
        }
    }
}
