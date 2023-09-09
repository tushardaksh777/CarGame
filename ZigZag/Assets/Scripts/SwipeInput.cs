using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static SwipeInput;
using static UnityEngine.ParticleSystem;

public class SwipeInput : MonoBehaviour
{
    public Rigidbody sphereRB;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;
    //public WheelColliders colliders;
    //public WheelMesh meshs;
    public float turnSpeed = 1.0f;
    public float movementSpeed;



    public GameObject[] wheels;
    public float rotationSpeed;
    
    public TrailRenderer[] trails;


    private bool recordRightSwipe;
    private bool recordLeftSwipe;

    public LayerMask wallLayer;
    public float tweenTime;

    private bool checkingForTurn;

    float rightHitDistance;
    float leftHitDistance;
    // Start is called before the first frame update

   

    public static event Action<Turn> T_turnDirection = delegate { };

    bool T_turnHit;

    public static bool carTurned = false;

    public enum Turn
    {
        Right,
        Left,
    }

    public enum carSides
    {
        left,
        right,
        center
    }
    public static Turn turn;
    public carSides sides;

    public enum carForwardAxis
    {
        x,
        NegativeX,
        z,
        NegativeZ

    }
    public static carForwardAxis carAxis;
    float turnDegree = 0;
    

    void Start()
    {
        dragDistance = Screen.height * 5 / 100;
        turnDegree = transform.rotation.eulerAngles.y;
        T_turnDirection = FloorGenration.instance.getT_turnDirection;
        //getPlayerForwordAxis();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!checkingForTurn)
        {
            turnController();
        }
        
        foreach (var wheel in wheels)
        {
            wheel.transform.Rotate(Time.deltaTime * 1 * rotationSpeed, 0, 0, Space.Self);

        }

        // Vector3 acc = Input.acceleration;
        //transform.position = Vector3.right * acc.x;
        if (Input.touchCount == 1){
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
                            recordRightSwipe = true;
                            recordLeftSwipe = false;

                            //rightSwipe();
                            //Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            recordLeftSwipe = true;
                            recordRightSwipe = false;

                            //leftSwipe();
                            //Debug.Log("Left Swipe");
                        }
                    }
                }
            }
            else
            {
                //applySteetAngle(0);
           }
         }
          

        checkCarDistanceSides();
        //GameManager.carStopped = !carTurned; 

    }
    private void FixedUpdate()
    {
        if (ControllerWithRigidBody.carGrounded && !checkingForTurn)
        {
           
            horizontalInput();
            
        }

    }

    void horizontalInput()
    {
        var input = Input.acceleration.x;
        //Debug.Log("Accelrometer " + input);
        turcar(input);

        
    }

    void checkCarDistanceSides()
    {
        RaycastHit righthit;
        RaycastHit lefthit;
        bool right,left;
        right = Physics.Raycast(transform.position, transform.right, out righthit, 4f, wallLayer);
        left = Physics.Raycast(transform.position, -transform.right, out lefthit, 4f, wallLayer);

        if (right)
        {
            //Debug.Log("Sides wall Right " +" distance "+righthit.distance);
            rightHitDistance = righthit.distance;
        }
        if (left)
        {
            leftHitDistance = lefthit.distance;
            //Debug.Log("Sides wall left "+ " distance " + lefthit.distance);
        }
        if(right && left)
        {
            setSides(righthit.distance , lefthit.distance);

           
            
           
        }
        
       
        

    }

    void setSides(float rightDistance , float leftDistance)
    {
        if(rightDistance == leftDistance)
        {
            //Center
            sides = carSides.center;
        }else if(rightDistance > 2.2f)
        {
            //left
            sides = carSides.left;
        }else if(leftDistance > 2.2f)
        {
            //right
            sides = carSides.right;
        }
        else
        {
            sides = carSides.center;
        }
        
        //if(righ)
    }

        public void turnController()
        {
        checkingTurn();
        

         checkForDrift();

       
         }
    bool checkingTurn()
    {
        bool isTurn;
        RaycastHit hit;
        GameObject hitObject;

        
        isTurn = Physics.Raycast(transform.position, transform.forward, out hit, 4f, wallLayer);
        if (isTurn)
        {
             hitObject  = hit.collider.gameObject.transform.parent.gameObject;
            if (hitObject.name == "Left_Turn")
            {
                if (recordRightSwipe && !recordLeftSwipe)
                {
                    //Tween for Turn Left
                    isTurn = false;
                    

                }
                

            }
            if (hitObject.name == "Right_Turn")
            {

                if (recordLeftSwipe && !recordRightSwipe)
                {
                    isTurn = false;
                }
                
            }
            //Debug.Log("Hit object " + hitObject.name);
            if (hitObject.name == "T_Turn1")
            {

                T_turnHit = true;
                
            }

            //Debug.Log("Ray Distance " + hit.distance);
            startTurn(isTurn, hit.distance);
        }
        



        return isTurn;
    }

    void startTurn(bool isCarNearOfTurn, float distance)
    {
        if (isCarNearOfTurn)
        {
            

            if (recordRightSwipe && !recordLeftSwipe)
            {

                rightSwipe(distance);
                carTurned = true;
                //GameManager.carStopped = false;

            }
            else if (recordLeftSwipe && !recordRightSwipe)
            {
                leftSwipe(distance);
                carTurned = true;
                //GameManager.carStopped = false;

            }
            else
            {
                GameManager.carStopped = true;
            }


        }
    }


    void checkForDrift()
    {
        bool isTurn;
        RaycastHit hit;
        GameObject hitObject;
        bool drift = false;


        isTurn = Physics.Raycast(sphereRB.transform.position, transform.forward, out hit, 5f, wallLayer);

        if (isTurn)
        {
            hitObject = hit.collider.gameObject.transform.parent.gameObject;
            if (hitObject.name == "Right_Turn")
            {
                if ((recordRightSwipe && !recordLeftSwipe) )
                {
                    if(hit.distance > 3.0f && tweenTime > 0.25f)
                    {
                        CameraAnimation.instance.startRightDriftAni();
                    }

                    

                }
            }

            if (hitObject.name == "Left_Turn")
            {
                if ((recordRightSwipe && !recordLeftSwipe) )
                {
                    //Tween for Turn Left
                    //drift = true;
                    if (hit.distance > 3.0f && tweenTime > 0.25f)
                    {
                        CameraAnimation.instance.startLeftDriftAni();
                    }
                    
                }

            }
        }

        
    }

    /*void leftSwipe(){
       Vector3 rotate = transform.localEulerAngles + (Vector3.up * -1* 90);
       Debug.Log("Rotation right "+transform.localEulerAngles.y);
       LeanTween.rotate(gameObject , rotate ,0.3f);
       applySteetAngle(-1);
   }
    void rightSwipe(){

       Vector3 rotate = transform.localEulerAngles + (Vector3.up * 1* 90);
       Debug.Log("Rotation right "+transform.localEulerAngles.y);
       LeanTween.rotate(gameObject , rotate ,0.3f);
           applySteetAngle(1);
       }*/

    void leftSwipe(float distance)
    {
        checkingForTurn = true;

        if (T_turnHit)
        {
            T_turnDirection(Turn.Left);
            T_turnHit = false;
        }
        calculateAxisDegree(-90f);
        setCarTurnTime(Turn.Left , distance);
        carTurned = true;
        //rotation to left 90 degree and Trail for drifting effect
        Vector3 rotate = transform.localEulerAngles + (Vector3.up * -1 * 90);
        //Debug.Log("Rotation right " + transform.localEulerAngles.y);
        startTrail();
        CarAnimation.instance.moveWheelsToRight();
        LeanTween.rotate(gameObject, rotate, tweenTime).setOnComplete(() =>
        {
            CameraAnimation.instance.backToNormalCameraAnimation();
            CarAnimation.instance. moveWheelsToStraight();
            stopTrail();
            checkingForTurn = false;
            recordLeftSwipe = false;
            recordRightSwipe = false;
            turn = Turn.Left;
            carTurned = false;
        });


    }
    void rightSwipe(float distance)
    {
        checkingForTurn = true ;

        if (T_turnHit)
        {
            T_turnDirection(Turn.Right);
            T_turnHit = false;
        }
        
        calculateAxisDegree(90f);
        setCarTurnTime(Turn.Right , distance);
        
        //CameraAnimation.instance.startDriftAni();
        //rotation to Right 90 degree and Trail for drifting effect
        Vector3 rotate = transform.localEulerAngles + (Vector3.up * 1 * 90);
        //Debug.Log("Rotation right " + transform.localEulerAngles.y);
        startTrail();
        CarAnimation.instance.moveWheelsToRight();
        carTurned = true;
        LeanTween.rotate(gameObject, rotate, tweenTime).setOnComplete(() =>
        {
            CameraAnimation.instance.backToNormalCameraAnimation();
            CarAnimation.instance.moveWheelsToStraight();
            stopTrail();
            checkingForTurn = false;
            recordLeftSwipe = false;
            recordRightSwipe = false;
            turn = Turn.Right;
            carTurned = false;
        });

    }

    void setCarTurnTime(Turn t  ,float distance)
    {
        if(distance > 3)
        {
            if (sides == carSides.center)
            {
                tweenTime = 0.3f;
            }
            else if (sides == carSides.right)
            {
                if (t == Turn.Right)
                {
                    tweenTime = 0.15f;
                }
                if (t == Turn.Left)
                {
                    tweenTime = 0.6f;
                }
            }
            else
            {
                if (t == Turn.Right)
                {
                    tweenTime = 0.6f;
                }
                if (t == Turn.Left)
                {
                    tweenTime = 0.15f;
                }
            }

        }
        else
        {
            tweenTime = 0.15f;
        }

        
    }
    /*void applySteetAngle(float input)
    {

        float SteeringAngle = 30 * 2 * input;
        colliders.FrontLeftWheel.steerAngle = SteeringAngle;
        colliders.FrontRightWheel.steerAngle = SteeringAngle;
    }*/



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

/*    public void moveWheelsToRight()
    {
        animator.SetBool("left", false);
        animator.SetBool("right", true);
    }

    public void moveWheelsToLeft()
    {
        animator.SetBool("left", true);
        animator.SetBool("right", false);
    }
    public void moveWheelsToStraight()
    {
        animator.SetBool("left", false);
        animator.SetBool("right", false);
    }
*/

    public void leftmove()
    {
        //Debug.Log("Left");
        turcar(-1f);
    }

    public void rightmove()
    {
        //Debug.Log("Right");
        turcar(1f);
    }

    void turcar(float Input)
    {

        // Calculate Turning Rotation
        //float newRot = Input * turnSpeed * Time.deltaTime;
        //transform.Rotate(0, newRot, 0, Space.World);
        //sphereRB.MovePosition(Vector3.right * Input * movementSpeed);
        if(Input > 0)
        {
            if(rightHitDistance > 0.8f)
            {
                //sphereRB.transform.position += transform.right * Input * movementSpeed;
                sphereRB.AddForce(transform.right * Input * movementSpeed , ForceMode.VelocityChange);
                //sphereRB.MovePosition(Vector3.right * Input * movementSpeed);
            }
            else
            {
                //sphereRB.velocity = Vector3.right * 0;
                //sphereRB.AddForce(transform.right * -Input * movementSpeed, ForceMode.Force);
            }
        }
        if(Input < 0)
        {
            if (leftHitDistance > 0.8f)
            {
                //sphereRB.transform.position += transform.right * Input * movementSpeed;
                sphereRB.AddForce(transform.right * Input * movementSpeed, ForceMode.VelocityChange);
                //sphereRB.MovePosition(Vector3.right * Input * movementSpeed);
            }
            else
            {
                //sphereRB.velocity = Vector3.right * 0;
                // sphereRB.AddForce(transform.right * -Input * movementSpeed, ForceMode.Force);
            }
        }

    }


    void calculateAxisDegree(float degree)
    {
        turnDegree += degree;
        if(turnDegree < 0)
        {
            if(turnDegree == -90)
            {
                turnDegree = 270;
            }else if(turnDegree == -180)
            {
                turnDegree = 180;
            }else if(turnDegree == -270)
            {
                turnDegree = 90;
            }
        }else if(turnDegree >= 360 || turnDegree <= -360)
        {
            turnDegree = 0;
        }

        setPlayerForwordAxis(turnDegree);
    }

    void setPlayerForwordAxis(float rotation)
    {
        
        //if(turnDegree = )
        //float rotation = gameObject.transform.rotation.eulerAngles.y;
        if (rotation == 0)
        {
            //Debug.Log("Z axis");
            carAxis = carForwardAxis.z;
            //z axis
        }
        else if (rotation == 90)
        {
            //Debug.Log("X axis");
            carAxis = carForwardAxis.x;
            //x axis
        }
        else if (rotation == 180)
        {
            //Debug.Log(" -Z axis");
            carAxis = carForwardAxis.NegativeZ;
            // -z axis
        }
        else if (rotation == 270)
        {
            //Debug.Log(" -X axis");
            carAxis = carForwardAxis.NegativeX;
            // -x axis
        }

       
    }
}
