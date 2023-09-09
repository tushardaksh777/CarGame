using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionEffect : MonoBehaviour
{
    public GameObject damageEffect;
    public Rigidbody sphereRb;
    public GameObject car;
    public GameObject camera;
    public GameObject headlights;

    public static bool rampStart = false;
    public static bool bridgeStart = false;

    int blastcar=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = car.transform.rotation; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ControllerWithRigidBody.forDevice)
        {
            if (collision.collider.tag == "sides_Wall")
            {
                if (sphereRb.velocity.x ==0 && sphereRb.velocity.z == 0)
                {

                    //stop = true;
                    GameManager.stop = true;
                }
                if (GameManager.carStopped && !SwipeInput.carTurned)
                {
                    hitEffect(collision);
                    new WaitForSeconds(0.5f);
                    if (GameManager.carStopped)
                    {
                        GameManager.stop = true;
                    }
                }
                

            }
            if(collision.collider.tag == "BlastCar")
            {
                blastcar++;
                hitEffect(collision);
                new WaitForSeconds(1.0f);
                if(blastcar > 0)
                {
                    GameManager.stop = true;
                }
                //GameManager.stop = true;
            }
            
        }
        else
        {
           // hitEffect(collision);
        }

        switch (collision.collider.tag)
        {
            case "ramp":
                rampTouched(false);
                break;

            case "obstacle":
                StartCoroutine(obstacleTouched(collision));
                break;
        }

       

        //if(collision.collider.tag == "floor")
        //{
         //   sphereRb.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //}
        //Debug.Log("Car t " + collision.collider.tag);

    }


    private void OnCollisionExit(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case "ramp":
                rampTrigger(false);
                //StartCoroutine(decreaseSpeed());
                break;
            case "BlastCar":
                blastcar--;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ramp":
                //rampTouched(false);
                break;
            case "cave_start":
                onCaveStart();
                break;
            case "cave_end":
                onCaveEnd();
                break;
            case "bridge":
                rampTouched(true);
                break;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        switch(other.tag)
        {
            case "ramp":
                //rampTrigger(false);
                break;
            case "bridge":
                rampTrigger(true);
                break;
           

        }
    }

    void rampTrigger(bool bridge)
    {
        rampStart = true;
        if (bridge)
        {
            bridgeStart = true;
        }
    }
    void rampTouched(bool bridge)
    {
        
        if(bridge)
        {
            ControllerWithRigidBody.forceSpeed = 85f;
            CarAnimation.instance.stopCarShocksAnimation();
            CameraAnimation.instance.startBridgeAnimation();

        }
        else
        {
            ControllerWithRigidBody.forceSpeed = 80f;
            CarAnimation.instance.stopCarShocksAnimation();
        }
        
            
    }
    IEnumerator decreaseSpeed(bool bridge)
    {
        yield return new WaitForSeconds(0.5f);
        ControllerWithRigidBody.forceSpeed = 30f;
        CarAnimation.instance.moveWheelsToStraight();
        if (bridge)
        {
            CameraAnimation.instance.stopBridgeAnimation();
        }

    }

    IEnumerator obstacleTouched(Collision collision)
    {
        hitEffect(collision);
         yield return new WaitForSeconds(1f);
        //GameManager.stop = true;

    }


    void hitEffect(Collision collision)
    {
        Instantiate(damageEffect, collision.GetContact(0).point, Quaternion.identity);
    }

    void onCaveStart()
    {
        headlights.SetActive(true);
        Debug.Log("CAVE START "+ camera.transform.position.z);
        //LeanTween.moveZ(camera, 3.4f, 1f);
        //LeanTween.move(camera, new Vector3(camera.transform.position.x,0.9f ,3.0f), 1f);
        LeanTween.moveLocal(camera, new Vector3(camera.transform.localPosition.x, 0.5f, -1.4f), 1f);

    }
    void onCaveEnd()
    {
        headlights.SetActive(false);
        LeanTween.moveLocal(camera, new Vector3(camera.transform.localPosition.x, 1.4f, -2.3f), 1f);
        //LeanTween.move(camera, new Vector3(camera.transform.position.x, 1.4f, 5.5f), 1f);
        //LeanTween.moveY(camera, camera.transform.position.y + 1.0f, 1f);
        // LeanTween.moveLocalZ(camera, -2.3f, 1f);
    }
}
