using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    private Rigidbody rb3D;
    private Steering[] steerings;

    public float maxVelocity = 10f;
    public float maxAngularvelocity = 3f;

    void Start() 
    { 
        rb3D = GetComponent<Rigidbody>(); steerings = GetComponents<Steering>(); 
    }

    void FixedUpdate()
    {
        AIBehaviour();
    }

    private void AIBehaviour()
    {
        Vector3 accelaration = Vector3.zero; float rotation = 0f;

        foreach (Steering behavior in steerings)
        {
            SteeringValues steering = behavior.GetSteering(this); accelaration += steering.CurrentVelocity; 
            rotation += steering.angular;
        }

        if (accelaration.magnitude > maxVelocity)
        {
            accelaration.Normalize(); accelaration *= maxVelocity;
        }

        rb3D.AddForce(accelaration);

        if (rotation != 0)
        {
            rb3D.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
