using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringValues
{
    public Vector3 CurrentVelocity; 
    public float angular; 
    public SteeringValues() { CurrentVelocity = Vector3.zero; angular = 0f; }
}
