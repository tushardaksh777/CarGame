using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampGenration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "player")
        {
            Debug.Log("Ramp ");

            ControllerWithRigidBody.forceSpeed = 120f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "player")
        {
            ControllerWithRigidBody.forceSpeed = 30f;
        }
    }

}
