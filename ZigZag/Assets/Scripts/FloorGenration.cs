using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static SwipeInput;

public class FloorGenration : MonoBehaviour
{
    public GameObject platform;

    public GameObject T_TurnObject;
    public Transform lastTransform;
    public Vector3 lastPos;
    public GameObject player;
    public GameObject LeftTurn;
    public GameObject RightTurn;
    public float genTiming;
    Vector3 prePos;
    public GameObject demotitionPlatform;//for demotitionPlatform we need to add 10 first and then instantiate after that add 9 in x or y position for next platform genration
    public GameObject ramp;
    bool T_Turn = false;
    public static Turn turnForT_Turn;

    public float directionSwitchTiming = 10.0f;
    public float T_TurnGenTiming = 4.0f;

    public float deletionTiming = 2.0f;

    int genDirection = -1;//if genDirection  1 means right and -1 means left;
    int directionTurn = 2;//0 means straight , 1 means right direction , -1 means left direction , 0 means both Directon (T_Turn)

    float platformRotation = 0f;



    bool rampGen; // for Ramp Genration on platform
    int afterRampPlatform = 0; // for platforms that will genrate after ramp genration
    bool rampStartedGen; // from this we identify ramp genrated and afterRampPlatform successfully genrated or not
    public float rampTiming = 10; // we use random range for get random timing


    bool demolishPlatformGen;
    public float demolishPlatformGenTiming = 10;


    //Obstacal 1
    public GameObject[] obstaclesList; // add 9.0f after Instantiateing object
    public int floorListCount;
    

    //Starting genration
    public float strightGenTiming = 30f;


    public carForwardAxis genAxis;


    public static FloorGenration instance;
    // Start is called before the first frame update

    public LayerMask groundLayer;

    private GameObject lastObstacle = null;
    public enum floorGenTurn
    {
        right,
        left,
        T_Turn,
        forward
    }
    private List<floorGenTurn> antiOverlapList;
    void Start()
    {
        //T_turnDirection = getT_turnDirection;
        getInstance();
        lastPos = lastTransform.position;
        genAxis = carForwardAxis.z;
        
        StartCoroutine(genPlatform());
        StartCoroutine(firstGen());
        //StartCoroutine(deletion());

        //StartCoroutine(rampGenration());
        //StartCoroutine(demolishGenration());

        

    }
    void getInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void gen()
    {

        prePos = lastPos;
        /*if(genDirection == 1){
           prePos.x += 3.0f;
           platformRotation = 90f;
           
        }
        if(genDirection == -1){
           prePos.z += 3.0f;
           platformRotation =0f;
        }*/
        //Debug.Log("Gen axis " + genAxis);
        if (genAxis == carForwardAxis.x)
        {
            prePos.x += 12.0f;
            platformRotation = 90f;

        } else if (genAxis == carForwardAxis.NegativeX)
        {
            prePos.x -= 12.0f;
            platformRotation = -90f;

        } else if (genAxis == carForwardAxis.z)
        {
            prePos.z += 12.0f;
            platformRotation = 0f;

        } else
        {
            prePos.z -= 12.0f;
            platformRotation = 180f;
        }


    }

    void deletePrefabs() {

        var child = gameObject.transform.GetChild(0).gameObject;
        if (child.transform.position.z < player.transform.position.z)
        {
            if (child.GetComponent<Rigidbody>() == null)
            {
                child.AddComponent<Rigidbody>();
                Destroy(child);
            }

        }
    }

    /* IEnumerator genPlatform()
     {
         while (!GameManager.stop)
         {
             gen();
             Quaternion rotation1 = Quaternion.Euler(0f,platformRotation,0f);

             if(directionTurn == 1 && !rampStartedGen)
             {
             Instantiate(RightTurn, prePos, Quaternion.identity, this.gameObject.transform);
             directionTurn=0;
             genDirection = 1;
             prePos.z +=3.0f;
             prePos.x +=3.0f;

             }else if(directionTurn == -1 && !rampStartedGen)
             {
             Instantiate(LeftTurn, prePos, Quaternion.Euler(0f,90f,0f), this.gameObject.transform);
             directionTurn=0;
             genDirection = -1;
             prePos.z += 3.0f;
             prePos.x += 3.0f;
             }
             else{
             GameObject current =Instantiate(platform, prePos, rotation1, this.gameObject.transform);

                 //int range = Random.Range(0, 20);
                 //comparing ramp and demolish platform gen so that both will not collide with each other

                 if(rampGen && !demolishPlatformGen)
                 {
                     //ramp gen
                     addRampOnFloor(current, rotation1);
                 }else if(!rampGen && demolishPlatformGen)
                 {
                     //demolish gen
                     addDemolishPlatform(rotation1);
                 }
                 else
                 {
                     int select = UnityEngine.Random.Range(0, 2);
                     if(select == 0)
                     {
                         addRampOnFloor(current, rotation1);
                         demolishPlatformGen = false;
                     }
                     else
                     {
                         addDemolishPlatform(rotation1);
                         rampGen = false;
                     }

                 }

                 //ramp Genration


             }



             lastPos = prePos;
             //deletePrefabs();
             yield return new WaitForSeconds(0.2f);
         }
     }*/
     
    IEnumerator deletion()
    {
        while (!GameManager.stop)
        {
            deletePlatform();

            yield return new WaitForSeconds(deletionTiming);
        }
    }

    IEnumerator genPlatform()
    {
        while (!GameManager.stop)
        {
            //Here we will check is car near to end or not if yes then we wil start genration if not then we stop genration

            //bool gen1 = checkingForCarPosition();
            //check active platform
            
            if (!T_Turn && gameObject.transform.childCount < floorListCount)
            {


                
                gen();
                
                Quaternion rotation1 = Quaternion.Euler(0f, platformRotation, 0f);

                if (directionTurn == 1)
                {
                    //deleteObjects();
                    Instantiate(RightTurn, prePos, rotation1, this.gameObject.transform);
                    carForwardAxis axis = changeGenAxis(directionTurn);
                    addPositionAfterPlatformGen(3.0f, 3.0f, axis);
                    genAxis = axis;
                    directionTurn = 2;//we are changing direction turn to 2 becoz we don't want to generate again this turn so 2 means normal platform will generate in genAxis direction
                   
                    //prePos.z += 3.0f;
                    //prePos.x += 3.0f;

                }
                else if (directionTurn == -1)
                {
                   // deleteObjects();
                    Instantiate(LeftTurn, prePos, rotation1, this.gameObject.transform);
                    carForwardAxis axis = changeGenAxis(directionTurn);
                    addPositionAfterPlatformGen(3.0f, 3.0f, axis);
                    genAxis = changeGenAxis(directionTurn);
                    directionTurn = 2; //we are changing direction turn to 2 becoz we don't want to generate again this turn so 2 means normal platform will generate in genAxis direction
                    //deletePlatform();
                    //prePos.z += 3.0f;
                    //prePos.x += 3.0f;
                }
                else if (directionTurn == 0)
                {
                   // deleteObjects();
                    //addPositionToGenAxisPlatform(1.0f);
                    Instantiate(T_TurnObject, prePos, rotation1, this.gameObject.transform);
                    // add 4 on previous direction , add 7 to direction in which car will turn
                    T_Turn = true;
                    directionTurn = 2; //we are changing direction turn to 2 becoz we don't want to generate again this turn so 2 means normal platform will generate in genAxis direction
                    //deletePlatform();
                    //prePos.z += 3.0f;
                    //prePos.x += 3.0f;

                }
                else
                {
                    
                    if (UnityEngine.Random.Range(0,2) == 0)
                    {
                        //normal Platform
                       // deleteObjects();
                        Instantiate(platform, prePos, rotation1, this.gameObject.transform);
                        lastObstacle = platform;
                       // deletePlatform();
                    }
                    else
                    {
                        //platform with obstacles
                        //deleteObjects();
                        addObstacles(rotation1);
                        //deletePlatform();
                    }
                    

                   
                }



                lastPos = prePos;
                //deletePrefabs();
                yield return new WaitForSeconds(genTiming);
            }
            else
            {
                yield return new WaitForSeconds(genTiming);
            }
           // deletePlatform();


        }
    }
    /* IEnumerator genswitch()
     {
         while (!GameManager.stop)
         {
             int random = UnityEngine.Random.Range(0, 2);
             if (!rampStartedGen)
             {
                 if (random > 0)
                 {
                     //genDirection = 1;
                     if (genDirection == 1)
                     {
                         directionTurn = -1;
                     }
                     else
                     {
                         directionTurn = 1;
                     }
                 }
                 else
                 {
                     // genDirection = -1;
                     if (genDirection == -1)
                     {
                         directionTurn = 1;
                     }
                     else
                     {
                         directionTurn = -1;
                     }
                 }
             }
             //Debug.Log("Switch "+genDirection);
             //deletePrefabs();
             yield return new WaitForSeconds(directionSwitchTiming);
         }
     }*/

    IEnumerator genswitch()
    {
        while (!GameManager.stop)
        {
            if(!T_Turn)
            {
                int random = UnityEngine.Random.Range(-1, 2);

                if (random == 0)
                {
                    //Both Direction (T_Turn)
                    directionTurn = 0;
                }
                else if (random == 1)
                {
                    //Right Direction
                    directionTurn = 1;
                }
                else
                {
                    //Left Direction
                    directionTurn = -1;
                }

                //antiOverlap(random);
            }
            //Debug.Log("Switch "+genDirection);
            //deletePrefabs();
            yield return new WaitForSeconds(directionSwitchTiming);
        }
    }

    IEnumerator turngen()
    {
        while (!GameManager.stop)
        {
            // int random = Random.Range(0, 2);
            // if(random > 0){
            //     directionTurn;
            // }else{
            //     RightTurn = true;
            // }
            //deletePrefabs();
            yield return new WaitForSeconds(T_TurnGenTiming);
        }
    }

    IEnumerator rampGenration()
    {
        while (!GameManager.stop)
        {
            //int random = Random.Range(0, 2);
            //if(random > 0){
            //    directionTurn;
            //}else{
            //    RightTurn = true;
            //}
            //deletePrefabs();
            rampTiming = UnityEngine.Random.Range(rampTiming, (rampTiming + rampTiming));
            rampGen = true;
            rampGen = true;
            yield return new WaitForSeconds(rampTiming);
        }
    }
    IEnumerator demolishGenration()
    {
        while (!GameManager.stop)
        {
            // int random = Random.Range(0, 2);
            // if(random > 0){
            //     directionTurn;
            // }else{
            //     RightTurn = true;
            // }
            //deletePrefabs();
            demolishPlatformGenTiming = UnityEngine.Random.Range(demolishPlatformGenTiming, (demolishPlatformGenTiming + demolishPlatformGenTiming));
            demolishPlatformGen = true;
            yield return new WaitForSeconds(rampTiming);
        }
    }
    IEnumerator firstGen()
    {

        yield return new WaitForSeconds(strightGenTiming);
        //Debug.Log("Direst gen completed");
        StartCoroutine(genswitch());
    }


    Quaternion getPlayerRotation() {
        Quaternion rotationY = player.transform.rotation;
        return rotationY;
    }


    void addRampOnFloor(GameObject parent, Quaternion rotation)
    {
        if (rampGen && !rampStartedGen)
        {
            GameObject rampIns = Instantiate(ramp, prePos, rotation, this.gameObject.transform);
            rampIns.transform.parent = parent.transform;
            var row = UnityEngine.Random.Range(0, 3);
            float rampPosi;
            if (row == 0)
            {
                rampPosi = -0.25f;
            } else if (row == 1)
            {
                rampPosi = 0.5f;
            }
            else
            {
                rampPosi = 1.25f;
            }
            rampIns.transform.position += new Vector3(rampPosi, 0.52f, -1f);
            //
            rampStartedGen = true;
            afterRampPlatform = 5;
            rampGen = false;
            /*if (rampGen)
            {
                //rampGen = false;
            }*/
        }
        if (afterRampPlatform > 0)
        {
            afterRampPlatform -= 1;
        }
        if (afterRampPlatform <= 0)
        {
            rampStartedGen = false;

        }
    }

    void addDemolishPlatform(Quaternion rotation)
    {

        //for Demolish Platform firstly we add 10 and then create after that add 9
        if (demolishPlatformGen)
        {
            demolishPlatformGen = false;
            if (genDirection == 1)
            {
                prePos.x += 10.0f;
            }
            if (genDirection == -1)
            {
                prePos.z += 10.0f;
            }

            Instantiate(demotitionPlatform, prePos, rotation, this.gameObject.transform);

            if (genDirection == 1)
            {
                prePos.x += 9.0f;
            }
            if (genDirection == -1)
            {
                prePos.z += 9.0f;
            }


            //Debug.Log("demplish platform genrated");
        }

    }

    carForwardAxis changeGenAxis(int direction)
    {
        // 1 for right direction , -1 for left direction and 0 for both direction (T_Turn)
        if(genAxis == carForwardAxis.x)
        {
            if(direction == 1)
            {
                return carForwardAxis.NegativeZ;
                // -z direction
            }else if(direction == -1)
            {
                return carForwardAxis.z;
                // z direction
            }else
            {
                return 0;
                // -z and z direction
            }

        }else if(genAxis == carForwardAxis.NegativeX)
        {

            if (direction == 1)
            {
                return carForwardAxis.z;
                // z direction
            }
            else if (direction == -1)
            {
                return carForwardAxis.NegativeZ;
                // -z direction
            }
            else
            {
                return 0;
                // z and -z direction
            }

        }
        else if(genAxis == carForwardAxis.z)
        {
            if (direction == 1)
            {
                return carForwardAxis.x;
                // x direction
            }
            else if (direction == -1)
            {
                return carForwardAxis.NegativeX;
                // -x direction
            }
            else
            {
                return 0;
                // x and -x direction
            }

        }
        else
        {
            if (direction == 1)
            {
                return carForwardAxis.NegativeX;
                //-x direction
            }
            else if (direction == -1)
            {
                return carForwardAxis.x;
                // x direction
            }
            else
            {
                return 0;
                // -x and x direction
            }

        }
    }

    void addPositionToGenAxisPlatform(float posi)
    {
        switch (genAxis)
        {
            case carForwardAxis.x:
                prePos.x += posi;
                break;
            case carForwardAxis.NegativeX:
                prePos.x -= posi;
                break;
            case carForwardAxis.z:
                prePos.z += posi;
                break;
            case carForwardAxis.NegativeZ:
                prePos.z -= posi;
                break;


        }
        lastPos = prePos;
    }


    void addPositionAfterPlatformGen(float pre , float next ,carForwardAxis nextAxis)
    {

        if (genAxis == carForwardAxis.x)
        {
            prePos.x += pre;
            addNextPositionAfterPlatformGen(next , nextAxis);

        }
        else if (genAxis == carForwardAxis.NegativeX)
        {
            prePos.x -= pre;
            addNextPositionAfterPlatformGen(next, nextAxis);

        }
        else if (genAxis == carForwardAxis.z)
        {
            prePos.z += pre;
            addNextPositionAfterPlatformGen(next, nextAxis);
        }
        else
        {
            prePos.z -= pre;
            addNextPositionAfterPlatformGen(next, nextAxis);
        }
    }
    void addNextPositionAfterPlatformGen(float next ,carForwardAxis axis)
    {
        switch (axis)
        {
            case carForwardAxis.x:
                prePos.x += next;
                break;
            case carForwardAxis.NegativeX:
                prePos.x -= next;
                break;
            case carForwardAxis.z:
                prePos.z += next;
                break;
            case carForwardAxis.NegativeZ:
                prePos.z -= next;
                break;


        }
    }

    bool checkingForCarPosition( float distanceFromPlatform ,Vector3 platform)
    {
        Vector3 carPosi = player.transform.position;
        if(genAxis == carForwardAxis.x)
        {
            if(carPosi.x > platform.x + distanceFromPlatform)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }else if(genAxis == carForwardAxis.NegativeX)
        {
            if (carPosi.x < platform.x - distanceFromPlatform)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else if(genAxis == carForwardAxis.z)
        {
            if (carPosi.z > platform.z + distanceFromPlatform)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else if(genAxis == carForwardAxis.NegativeZ)
        {
            if (carPosi.z < platform.z - distanceFromPlatform)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public  void getT_turnDirection(Turn turn)
    {
        //carForwardAxis axis = changeGenAxis(directionTurn);
        if (T_Turn)
        {
            
           // Debug.Log("T_Turn " + turn);
            addPositionToGenAxisPlatform(3.0f);
            addPosiTOT_TurnDirection(turn);
            gen();
            Instantiate(platform, prePos, Quaternion.Euler(0f, platformRotation, 0f), this.gameObject.transform);
            lastPos = prePos;
            T_Turn = false;
            

        }
       
    }

    void addPosiTOT_TurnDirection(Turn turn)
    {
        switch (genAxis)
        {
            case carForwardAxis.x:
                if (turn == Turn.Right)
                {
                    // -z direction
                    genAxis = carForwardAxis.NegativeZ;
                    prePos.z -= 3.0f;
                    
                }
                else
                {
                    // z direction
                    genAxis = carForwardAxis.z;
                    prePos.z += 3.0f;
                    
                }
                break;
            case carForwardAxis.NegativeX:
                if (turn == Turn.Right)
                {
                    //z direction
                    genAxis = carForwardAxis.z;
                    prePos.z += 3.0f;
                    
                }
                else
                {
                    // -z direction
                    genAxis = carForwardAxis.NegativeZ;
                    prePos.z -= 3.0f;
                   
                }
                break;
            case carForwardAxis.z:
                if (turn == Turn.Right)
                {
                    // x direction
                    genAxis = carForwardAxis.x;
                    prePos.x += 3.0f;
                   
                }
                else
                {
                    // -x direction
                    genAxis = carForwardAxis.NegativeX;
                    prePos.x -= 3.0f;
                    
                }
                break;
            case carForwardAxis.NegativeZ:
                if (turn == Turn.Right)
                {
                    // -x direction
                    genAxis = carForwardAxis.NegativeX;
                    prePos.x -= 3.0f;
                   
                }
                else
                {
                    //x direction
                    genAxis = carForwardAxis.x;
                    prePos.x += 3.0f;
                    
                }
                break;


        }
        lastPos = prePos;


    }



    void addObstacles(Quaternion rotation)
    {
        //Debug.Log("Obstacles " + rotation);
        int platformIndex = UnityEngine.Random.Range(0, obstaclesList.Length);
        GameObject Obstacle = obstaclesList[platformIndex];

        Obstacle = findObstacle(Obstacle);



        Instantiate(Obstacle, prePos, rotation, this.gameObject.transform);
        lastObstacle = Obstacle;
        //Debug.Log("Obstacles index " + platformIndex +" "+ Obstacle);

        //avoid repeating same object
        /*   if (lastObstacle != null && lastObstacle.name == Obstacle.name)
           {
               if(platformIndex + 1 >= obstaclesList.Length)
               {
                   Obstacle = obstaclesList[platformIndex - 1];
               }
               else
               {
                   Obstacle = obstaclesList[platformIndex + 1];
               }
           } */

        /* if (lastObstacle != null)
         {
             //prevent creating a ramp obstacle after already created ramp obs
             if (Obstacle.name == "obstacals_4" && lastObstacle.name == "obstacals_2")
             {
                 if (platformIndex + 1 >= obstaclesList.Length)
                 {
                     Obstacle = obstaclesList[platformIndex - 1];
                 }
                 else
                 {
                     Obstacle = obstaclesList[platformIndex + 1];
                 }
             }
             else if (Obstacle.name == "obstacals_2" && lastObstacle.name == "obstacals_4")
             {
                 if (platformIndex + 1 >= obstaclesList.Length)
                 {
                     Obstacle = obstaclesList[platformIndex - 1];
                 }
                 else
                 {
                     Obstacle = obstaclesList[platformIndex + 1];
                 }
             }else if(lastObstacle.name == "obstacals_4" && Obstacle.name == "obstacals_5")
             {
                 Obstacle = platform;
             }
         } */


        /* if(Obstacle.name == "obstacals_4")
         {
             addPositionToGenAxisPlatform(12.0f);
         }
         else
         {
             addPositionToGenAxisPlatform(9.0f);
         }
         switch(Obstacle.name)
         {
             case "obstacals_4":
                 addPlatform(2, rotation);
                 break;

             case "obstacals_2":
                 addPlatform(2, rotation);
                 break;
         }*/


    }

    void addPlatform(int count , Quaternion rotation)
    {
        for(int i = 0; i < count; i++)
        {
            gen();
            Instantiate(platform, prePos, rotation, this.gameObject.transform);
            lastPos = prePos;
        }
    }


    void checkForOverlapWalls  () 
    {
        
        if(directionTurn == 0)
        {
            //both Direction
            t_turnHit();
            

        }
        else if(directionTurn == 1)
        {
            //Right direction
            rightHit();

        }
        else if(directionTurn == -1)
        {
            //Left Direction
            leftHit();

        }
        else
        {
            forwardHit();
        }
    }
    void forwardHit()
    {
        bool ground;
        RaycastHit hit;
        Vector3 posi = addPosiForOverlapWallsHit(floorGenTurn.forward);
        ground = Physics.Raycast(prePos, posi, out hit, 10f, groundLayer);
        if(ground)
        {

            changeTurnGenrationDirection(checkforForwardConfirm());
        }
    }

    floorGenTurn checkforForwardConfirm()
    {
        //first we will hit for right
        bool Rground;
        RaycastHit Rhit;
        floorGenTurn Rturn;
        Vector3 posi = addPosiForOverlapWallsHit(floorGenTurn.right);
        Rground = Physics.Raycast(prePos, posi, out Rhit, 10f, groundLayer);
        if (Rground)
        {
            return floorGenTurn.left;
        }
        else
        {
            return floorGenTurn.right;
        }

    }

    void leftHit()
    {
        bool ground;
        RaycastHit hit;
        Vector3 posi = addPosiForOverlapWallsHit(floorGenTurn.left);
        ground = Physics.Raycast(prePos, posi, out hit, 10f, groundLayer);

        if (ground)
        {
            changeTurnGenrationDirection(floorGenTurn.left);
        }
    }

    void rightHit()
    {
        bool ground;
        RaycastHit hit;
        Vector3 posi = addPosiForOverlapWallsHit(floorGenTurn.right);
        ground = Physics.Raycast(prePos, posi, out hit, 10f, groundLayer);
        if (ground)
        {
            changeTurnGenrationDirection(floorGenTurn.left);
        }
    }
    void t_turnHit()
    {
        //here we hit ray cast in both direction and we need both direction hit
        bool left , right;
        RaycastHit lefthit,righthit;
        Vector3 leftposi = addPosiForOverlapWallsHit(floorGenTurn.left);

        Vector3 rightposi = addPosiForOverlapWallsHit(floorGenTurn.right);

        left = Physics.Raycast(prePos, leftposi, out lefthit, 10f, groundLayer);
        right = Physics.Raycast(prePos, rightposi, out righthit, 10f, groundLayer);

        if(left && !right)
        {
            //right turn
            changeTurnGenrationDirection(floorGenTurn.right);
        }
        else if(!left && right)
        {
            //left turn
            changeTurnGenrationDirection(floorGenTurn.left);
        }
        else if(left && right)
        {
            //stright
            changeTurnGenrationDirection(floorGenTurn.forward);
        }
    }

    void changeTurnGenrationDirection(floorGenTurn turn)
    {
        if(turn == floorGenTurn.right)
        {
            directionTurn = 1;
        }else if(turn == floorGenTurn.left)
        {
            directionTurn = -1;
        }else if(turn == floorGenTurn.forward)
        {
            directionTurn = 2;
        }
        else
        {

        }

    }

    Vector3 addPosiForOverlapWallsHit(floorGenTurn turn )
    {
        Vector3 hitposi = new Vector3();
        if (turn == floorGenTurn.right)
        {

            switch (genAxis)
            {
                case carForwardAxis.x:
                    hitposi = new Vector3(0,0,-1);
                    break;
                case carForwardAxis.NegativeX:
                    hitposi = new Vector3(0, 0, 1);
                    break;
                case carForwardAxis.z:
                    hitposi = new Vector3(1, 0, 0);
                    break;
                case carForwardAxis.NegativeZ:
                    hitposi = new Vector3(-1, 0, 0);
                    break;


            }
        }
        else if (turn == floorGenTurn.left)
        {
            switch (genAxis)
            {
                case carForwardAxis.x:
                    hitposi = new Vector3(0, 0, 1);
                    break;
                case carForwardAxis.NegativeX:
                    hitposi = new Vector3(0, 0, -1);
                    break;
                case carForwardAxis.z:
                    hitposi = new Vector3(-1, 0, 0);
                    break;
                case carForwardAxis.NegativeZ:
                    hitposi = new Vector3(1, 0, 0);
                    break;


            }
        }
        else
        {
            switch (genAxis)
            {
                case carForwardAxis.x:
                    hitposi = new Vector3(1, 0, 0);
                    break;
                case carForwardAxis.NegativeX:
                    hitposi = new Vector3(-1, 0, 0);
                    break;
                case carForwardAxis.z:
                    hitposi = new Vector3(0, 0, 1);
                    break;
                case carForwardAxis.NegativeZ:
                    hitposi = new Vector3(0, 0, -1);
                    break;


            }
        }

        return hitposi;
    }



    //Object Deletion

    void deletePlatform()
    {
        //bool ac = (gameObject.transform.childCount > 15) ? false : true;

        if (gameObject.transform.childCount > floorListCount - 1 )
        {
            GameObject d = gameObject.transform.GetChild(0).gameObject;
            d.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(d, 0.5f);

        }
       
       
    }


    //Anti Overlap Controller

    void antiOverlap(int data)
    {
        if(data == 0)
        {
            antiOverlapList.Add(floorGenTurn.T_Turn);
        }else if (data == 1)
        {
            antiOverlapList.Add(floorGenTurn.right);
        }else if(data == -1){
            antiOverlapList.Add(floorGenTurn.left);
        }
        if(antiOverlapList.Count > 0) {
            checkForSameDirection();
        }
        
    }

    //here we check which direction added two or more times ...if left direction added then we switch to right same goes for right
    void checkForSameDirection()
    {
        int c = 0;
        floorGenTurn t;
        if(antiOverlapList.Count > 2)
        {
           antiOverlapList.RemoveAt(antiOverlapList.Count - 1 ); 


        }
        if(antiOverlapList.Count == 2)
        {
            if (antiOverlapList[0] == antiOverlapList[1])
            {
                c = 1;
            }
            else
            {
                c = 0;
            }
        }
        

         if(c == 1)
        {
            t = antiOverlapList[0];
            if(t == floorGenTurn.right)
            {
                directionTurn = -1;

            }
            else if(t == floorGenTurn.left)
            {
                directionTurn = 1;

            }else if(t== floorGenTurn.T_Turn)
            {
                directionTurn = 2;
            }
        }
        
    /*    for(int i = 0; i < antiOverlapList.Count; i++)
        {
            if (antiOverlapList[i] == antiOverlapList[0])
            {
                c += 1;
                
            }
            else
            {
                c = 0;
            }
        }
        */

    }





    ///Rules for Obstacle genrations

    GameObject findObstacle(GameObject obj)
    {
        GameObject nextObj = obj;
        GameObject[] finallist;
        if(lastObstacle != null)
        {
            if(lastObstacle.name == "obstacals_2" && nextObj.name == "obstacals_4" || lastObstacle.name == "obstacals_4" && nextObj.name == "obstacals_2")
            {
                //avoiding double Ramp objects

                List<GameObject> list = new List<GameObject>();
                for(int i=0; i < obstaclesList.Length; i++)
                {
                    if (obstaclesList[i].name != "obstacals_4" || obstaclesList[i].name != "obstacals_2")
                    {
                        list.Add(obstaclesList[i]);
                    }
                }
                finallist = list.ToArray();
                int v =  UnityEngine.Random.Range(0, finallist.Length);
                nextObj = finallist[v];
            }
            
            if(lastObstacle.name == "obstacals_2"  && nextObj.name == "obstacals_5"  || lastObstacle.name == "obstacals_4" && nextObj.name == "obstacals_5")
            {
                //avoiding cave after Ramp ObjectList<GameObject> list = new List<GameObject>();
                List<GameObject> list = new List<GameObject>();
                for (int i = 0; i < obstaclesList.Length; i++)
                {
                    if (obstaclesList[i].name != "obstacals_5")
                    {
                        list.Add(obstaclesList[i]);
                    }
                }
                finallist = list.ToArray();
                int v = UnityEngine.Random.Range(0, finallist.Length);
                nextObj = finallist[v];
            }
            
            if(lastObstacle.name == "obstacals_2" && nextObj.name == "OpenBridge" || lastObstacle.name == "obstacals_4" && nextObj.name == "OpenBridge")
            {
                //avoiding Ramp Bridge after Ramp Object
                List<GameObject> list = new List<GameObject>();
                for (int i = 0; i < obstaclesList.Length; i++)
                {
                    if (obstaclesList[i].name != "OpenBridge")
                    {
                        list.Add(obstaclesList[i]);
                    }
                }
                finallist = list.ToArray();
                int v = UnityEngine.Random.Range(0, finallist.Length);
                nextObj = finallist[v];
            }
        }
        return nextObj;
    }
}
