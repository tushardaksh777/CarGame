using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{

    public float speed;
    bool turnRight = false;
    bool stop = false;
    public float cubeSize = 0.1f;
    float cubePivotDistance;
    Vector3 cubePivot;
    public int cubeInRow = 5;
    // Start is called before the first frame update
    void Start()
    {
        cubePivotDistance = cubeSize * cubeInRow;
        cubePivot = new Vector3(cubePivotDistance, cubePivotDistance, cubePivotDistance);   
    }

    // Update is called once per frame  
    void Update()
    {
        if(transform.position.y < -1 && !stop)
        {
            //gameObject.SetActive(false);
            //explode();

            stop = true;
            SceneManager.LoadScene("SampleScene");
        }
        
        input();
    }
    void LateUpdate()
    {
        move();
        
    }
    void explode()
        
    {
        for(int x = 0; x < cubeSize; x++)
        {
           for(int y=0;y < cubeSize; y++)
            {
                for(int z=0;z < cubeSize; z++)
                {
                    createExplodPiece(x,y,z);
                }
            }
        }

        Vector3 explosionPosition = transform.position;

    }
    void createExplodPiece(int x , int y , int z)
    {
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.transform.position = transform.position + new Vector3(cubeSize*x , cubeSize*y,cubeSize*z) - cubePivot;
        transform.transform.localScale = new Vector3(cubeSize,cubeSize,cubeSize);

        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }

    void move()
    {
       transform.position += transform.forward * speed*Time.deltaTime;

        
    }
    void input()
    {
        if (Input.GetMouseButtonDown(0))
        {
            turnplayer();
        }
    }
    void turnplayer()
    {
        if (turnRight)

        {
            turnRight = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
           
        }
        else
        {
            turnRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

/// <summary>
/// OnTriggerEnter is called when the Collider other enters the trigger.
/// </summary>
/// <param name="other">The other Collider involved in this collision.</param>
void OnTriggerEnter(Collider other)
{
    if(other.tag == "diamond")
        {
            
            GameManager.instance.setDiamond(1);
            //Debug.Log("Prefabs "+other.gameObject.name);
            Destroy(other.gameObject,0.1f);
            
        }
}
}
