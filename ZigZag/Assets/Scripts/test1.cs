using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 force;
    public float rbForce;
    public float speed=40f;
    public float initalSpeed =10f;
    public float drag= 0.98f;

    public float traction = 1;
    public WheelColliders colliders;
    public WheelMesh  meshs;
    public float steerAngle = 15;
    public float rotation =0.0f;
    bool stop = false;

    public static GameObject playerInstance = null;

    private Rigidbody rb;


    void getInstance()
    {
        if(playerInstance == null)
        {
            playerInstance = gameObject;
        }
    }
    void Start()
    {
        //StartCoroutine(InitialSpeed());
        getInstance();
         rb = transform.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
       
        rbForce = 1;
        rbForce *= speed;
        applyForwardForce();
        //swipeInput();
        //input();

    }
    private void FixedUpdate()
    {
        //force = Vector3.forward * speed;
        //rb.AddForce(force , ForceMode.Acceleration);

       

    }

    private void LateUpdate() {

        //Debug.Log("Veloctiy " + rb.velocity);

        // float steerInput = swipeInput();
        // if(steerInput == 1){

        // Vector3 rotate = transform.localEulerAngles + (Vector3.up * 1* 90);
        // Debug.Log("Rotation right "+transform.localEulerAngles.y);
        // LeanTween.rotate(gameObject , rotate ,0.3f);
        // //transform.Rotate(rotate);
        // }
        // if(steerInput == -1){
        //  Vector3 rotate = transform.localEulerAngles + (Vector3.up * -1  * 90 );
        // Debug.Log("Rotation left "+transform.localEulerAngles.y);
        // LeanTween.rotate(gameObject , rotate ,0.3f);
        // //transform.Rotate(rotate);
        // }

        // Vector3 rotate = Vector3.up * steerInput *force.magnitude * steerAngle *Time.deltaTime * 2;
        // transform.Rotate(rotate);



    }

    // private Vector3 getRotation(){
    //    int rotate = 180;
    //    Vector3 ro = Vector3.up *90;
    //    if()

    // }
    void applyForwardForce()
    {

        ///Using only transform.forward
        force += transform.forward * speed * Time.deltaTime;
        transform.position += force * Time.deltaTime;

        //rb.AddForce(force, ForceMode.Acceleration);



        force *= drag;
        force = Vector3.ClampMagnitude(force, steerAngle);


        force = Vector3.Lerp(force.normalized, transform.forward, traction * Time.deltaTime) * force.magnitude;
    }
        void applySteetAngle(float input){

        float SteeringAngle = steerAngle* 2 * input;
        colliders.FrontLeftWheel.steerAngle = SteeringAngle;
        colliders.FrontRightWheel.steerAngle= SteeringAngle;
    }
      void applyWorldPoistion(){
        updateWheels(colliders.FrontLeftWheel , meshs.FrontLeftWheel);
        updateWheels(colliders.FrontRightWheel , meshs.FrontRightWheel);
        updateWheels(colliders.RearLeftWheel , meshs.RearLeftWheel);
        updateWheels(colliders.RearRightWheel , meshs.RearRightWheel);
    }

        void updateWheels(WheelCollider collider , MeshRenderer mesh){
        
        Quaternion quat;
        Vector3 poistion;
        collider.GetWorldPose(out poistion , out quat);
        mesh.transform.position = poistion;
        rotation += 0.1f*speed*Input.GetAxis("Vertical");
        mesh.transform.rotation =  Quaternion.Euler(quat.eulerAngles.x+rotation , quat.eulerAngles.y ,quat.eulerAngles.z);   
}
void OnTriggerEnter(Collider other)
{
    if(other.tag == "diamond")
        {
            
            GameManager.instance.setDiamond(1);
            //Debug.Log("Prefabs "+other.gameObject.name);
            Destroy(other.gameObject,0.1f);
            
        }
}

//Increasing Initial Speed
// IEnumerator InitialSpeed(){
//     while(initalSpeed != speed){
//         initalSpeed += 5f;
//         yield return new WaitForSeconds(0.5f);
//     }
// }



 ////Swipe Input
 //Left swipe = 0 , right swipe = 1;
public void left(){
    Vector3 rotate = transform.localEulerAngles + (Vector3.up * -1* 90);
    Debug.Log("Rotation right "+transform.localEulerAngles.y);
    LeanTween.rotate(gameObject , rotate ,0.3f);
    applySteetAngle(1f);
    }
public void right(){
    Vector3 rotate = transform.localEulerAngles + (Vector3.up * 1* 90);
    Debug.Log("Rotation right "+transform.localEulerAngles.y);
    LeanTween.rotate(gameObject , rotate ,0.3f);
        applySteetAngle(-1f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "sides_Wall")
        {
            //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            
        }
    }

    //  private void swipeInput(){

    //     foreach (Touch touch in Input.touches) {
    // 			if (touch.phase == TouchPhase.Began) {
    // 				fingerUpPos = touch.position;
    // 				fingerDownPos = touch.position;
    // 			}

    // 			//Detects Swipe while finger is still moving on screen
    // 			if (touch.phase == TouchPhase.Moved) {
    // 				if (!detectSwipeAfterRelease) {
    // 					fingerDownPos = touch.position;
    // 					DetectSwipe ();
    // 				}
    // 			}

    // 			//Detects swipe after finger is released from screen
    // 			if (touch.phase == TouchPhase.Ended) {
    // 				fingerDownPos = touch.position;
    // 				DetectSwipe ();
    // 			}
    // 		}


    //  }


    // [System.Serializable]
    // public class WheelColliders
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
}

[System.Serializable]
public class WheelMesh
{
    public MeshRenderer FrontLeftWheel;
    public MeshRenderer FrontRightWheel;
    public MeshRenderer RearLeftWheel;
    public MeshRenderer RearRightWheel;
}
