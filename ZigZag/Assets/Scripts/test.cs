using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 newPosi;
    public float speed;

    public WheelColliders colliders;
    public WheelMesh  meshs;
    public float steerAngle;

    public AnimationCurve steerCurve;
    public float rotation =0.0f;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //moving forward 
        // newPosi += transform.forward * speed * Time.deltaTime;
        // transform.position += newPosi * Time.deltaTime;
        

        //controlling steer angle
        
        turnInput();

        
        applySteetAngle();
        

    }
    private void LateUpdate() {
        colliders.RearLeftWheel.motorTorque = 20 * speed;
        colliders.RearRightWheel.motorTorque = 20*speed;
        applyWorldPoistion();
        
    }
    
    void turnInput(){
     steerAngle = Input.GetAxis("Horizontal");
    }
    void applySteetAngle(){

        float SteeringAngle = steerAngle*steerCurve.Evaluate(0);
        colliders.FrontLeftWheel.steerAngle = SteeringAngle;
        colliders.FrontRightWheel.steerAngle=SteeringAngle;
     }

    private void FixedUpdate() {
        
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
        //mesh.transform.position = poistion;
        rotation += 5.0f;
        mesh.transform.rotation =  quat  ;
        }
}

// [System.Serializable]
// public class WheelColliders
// {
//     public WheelCollider FrontLeftWheel;
//     public WheelCollider FrontRightWheel;
//     public WheelCollider RearLeftWheel;
//     public WheelCollider RearRightWheel;
// }

// [System.Serializable]
// public class WheelMesh
// {
//     public MeshRenderer FrontLeftWheel;
//     public MeshRenderer FrontRightWheel;
//     public MeshRenderer RearLeftWheel;
//     public MeshRenderer RearRightWheel;
// }
