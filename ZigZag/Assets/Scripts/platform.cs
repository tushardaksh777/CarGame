using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SwipeInput;

public class platform : MonoBehaviour
{
    public GameObject diamond;
    GameObject player;

    private carForwardAxis carAxis;

    // Start is called before the first frame update
    void Start()
    {
        
        var i = Random.Range(0,10);
        Vector3 tempPos = transform.position;
        tempPos.y += 1;
        if( i==5){
            //Instantiate(diamond , tempPos , diamond.transform.rotation);
        }
        
    }
    private void OnCollisionExit(Collision other) {

        if(other.collider.tag == "player")
        {
            player = other.collider.gameObject;
            Debug.Log("Collide "+gameObject.name);
            //StartCoroutine(checkFordeletion(player));
            //Rigidbody rb = other.collider.GetComponent<Rigidbody>();
            //Debug.Log("Player velocity "+rb.velocity);
            
                //Invoke("objectFall", 1f);
            
                
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

/*    IEnumerator checkFordeletion(GameObject player)
    {
        while (!gameObject.IsDestroyed())
        {
            switch(carAxis)
            {
                case carForwardAxis.x:
                    if (player.transform.position.x > gameObject.transform.position.x + 3)
                    {
                        objectFall();
                    }
                    break;

                case carForwardAxis.NegativeX:
                    if (player.transform.position.x > gameObject.transform.position.x + 3)
                    {
                        objectFall();
                    }
                    break;

                case carForwardAxis.NegativeZ:
                    if (player.transform.position.x > gameObject.transform.position.x + 3)
                    {
                        objectFall();
                    }
                    break;

                case carForwardAxis.z:
                    if (player.transform.position.x > gameObject.transform.position.x + 3)
                    {
                        objectFall();
                    }
                    break;
            }
        }
    }*/

    void objectFall()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 0.5f);
    }
    void objectFall1()
    {
        
        if (gameObject.name.Equals("Right_Turn(Clone)"))
        {
           // Debug.Log("Platform name " + gameObject.name);
            if (player.transform.position.x > transform.position.x +4f)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Invoke("objectFall", 1f);
            }
        }else if (gameObject.name.Equals("Left_Turn(Clone)"))
        {
           // Debug.Log("Platform name " + gameObject.name);
            if (player.transform.position.z > transform.position.z + 4f)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(gameObject, 0.5f);
            }
            else
            {
                Invoke("objectFall", 1f);
            }
        }
        else if(gameObject.name.Equals("Platform2(Clone)"))
        {
            if(gameObject.transform.rotation.y == 0)
            {
                //Debug.Log("Platform name " + gameObject.name);
                if (player.transform.position.z > transform.position.z)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(gameObject, 0.5f);
                }
                else
                {
                    Invoke("objectFall", 1f);
                }
            }
            else
            {
                //Debug.Log("Platform name " + gameObject.name);
                if (player.transform.position.x > transform.position.x + 5)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(gameObject, 0.5f);
                }
                else
                {
                    Invoke("objectFall", 1f);
                }
            }
        }
        else { }
        
    }

   
}
