using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour
{
    public abstract SteeringValues GetSteering(Behaviour behaviour);
}
