using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerWithRigidBody : MonoBehaviour
{
    
    public Rigidbody sphereRB;
    public Rigidbody carRb;
    private float moveInput;
    public float speed;
    public static float forceSpeed;
    private float turnInput;
    public float turnSpeed;
    public float tweenTime;
    private bool isCarGrounded;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float airDrag;
    public float groundDrag;

    private float normalDrag;
    public float modifiedDrag;

    public float alignToGroundTime;


    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;


    public static bool forDevice;
    public static bool carGrounded;
    public bool fordeviceInput;

    public TrailRenderer[] trails;

    public static GameObject playerInstance =null;
    public static GameObject carInstance = null;

    private bool spwipeRecorded = false;
    public GameObject cameraSphere;

    void Start()
    {
        sphereRB.transform.parent= null;
        carRb.transform.parent= null;
        normalDrag = sphereRB.drag;
        stopTrail();
        getInstance();
        getCarInstance();
        forDevice = fordeviceInput;

        forceSpeed = speed;
        
        //WheelController.instance.
    }
    void getInstance()
    {
        if (playerInstance == null)
        {
            playerInstance = sphereRB.gameObject;
        }
    }
    void getCarInstance()
    {
        if(carInstance== null)
        {
            carInstance = gameObject;
        }
    }


    void Update()
    {
        speed = forceSpeed;
        carGrounded = isCarGrounded;
        Debug.Log("Velocity " + sphereRB.velocity );
        //Game Over and restart Scene 
        if (forDevice)
        {
            if (transform.position.y < -1)
            {
                //gameObject.SetActive(false);
                //explode();
                //Debug.Log("Game Over");

                GameManager.stop= true;
               
            }
        }
            
        //setting speed to Vertical input
        if (forDevice)
        {
            moveInput = 1;
            moveInput *= speed ;
        }
        else
        {
            moveInput = Input.GetAxisRaw("Vertical");
            moveInput *= speed;
        }

        //Getting Input horizontal Input
        if (forDevice)
        {
            //swipeInputForDevice();
        }
        else
        {
            /*turnInput = Input.GetAxisRaw("Horizontal");
            // Calculate Turning Rotation
            float newRot = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            if (isCarGrounded)
                transform.Rotate(0, newRot, 0, Space.World);
             */
        }

        //setting sphere position to their parent(car)
        transform.position = sphereRB.transform.position;
        //transform.position = transform.position;

        //Checking grounded with raycast
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, groundLayer);

        //Debug.Log("IS grounded " + isCarGrounded + " hit "+hit.normal);
       // Debug.Log("IS grounded rotation " + (Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation));

        //rotation when player is not on ground
       Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime*Time.deltaTime);
        Debug.Log(" Car Rotation " + toRotateTo.ToString());

        rampSpeedDecrease();


        //setting drag according to car poistion
        sphereRB.drag = isCarGrounded ? groundDrag : airDrag;

    }

    void turnCar(float input)
    {
        if (input > 0)
        {
            if (isCarGrounded)
                rightSwipe();
        }
        if (input < 0)
        {
            if (isCarGrounded)
                leftSwipe();
        }

    }
    private void LateUpdate()
    {

    }
    private void FixedUpdate()
    {
        

        //adding force to Car
        if (isCarGrounded)
        {
            sphereRB.AddForce((transform.forward * moveInput), ForceMode.Acceleration);
        }
        else
        {
            sphereRB.AddForce(transform.up * -10f);
        }

        //Moving rotation accordingly Horizontal input
        if (forDevice)
        {
            //carRb.MoveRotation(transform.rotation);
        }
        //turnCar(turnInput);
    }

    void leftSwipe()
    {
        //rotation to left 90 degree and Trail for drifting effect
        Vector3 rotate = transform.localEulerAngles + (Vector3.up * -1 * 90);
        //Debug.Log("Rotation right " + transform.localEulerAngles.y);
        startTrail();
        LeanTween.rotate(gameObject, rotate, tweenTime).setOnComplete(() =>
        {
            stopTrail();
        });
        
       
    }
    void rightSwipe()
    {
        //rotation to Right 90 degree and Trail for drifting effect
        Vector3 rotate = transform.localEulerAngles + (Vector3.up * 1 * 90);
        //Debug.Log("Rotation right " + transform.localEulerAngles.y);
        startTrail();
        LeanTween.rotate(gameObject, rotate, tweenTime).setOnComplete(() =>
        {
            stopTrail();
        });
       
    }


    void swipeInputForDevice() {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            turnCar(1);
                            //Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            turnCar(-1);
                            //Debug.Log("Left Swipe");
                        }
                    }
                }
            }
            
        }
        

    }

    bool checkingTurn()
    {
        bool isTurn;
        RaycastHit hit;

        isTurn = Physics.Raycast(transform.position, transform.forward, out hit, 7f, wallLayer);
        //Debug.Log("Ray Distance " + hit.distance);
        return isTurn;
    }

    public void startTrail()
    {
        foreach (var trail in trails)
        {
            trail.emitting = true;
        }
    }
    public void stopTrail()
    {
        foreach (var trail in trails)
        {
            trail.emitting = false;
        }
    }

    void rampSpeedDecrease()
    {
        if (isCarGrounded && CollisionEffect.rampStart && sphereRB.transform.position.y < 0.5f)
        {
            CollisionEffect.rampStart = false;
            forceSpeed = 30f;
            CarAnimation.instance.moveWheelsToStraight();
            if (CollisionEffect.bridgeStart)
            {
                CameraAnimation.instance.stopBridgeAnimation();
                CollisionEffect.bridgeStart= false;
            }
            
        }
    }
}
