using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private Rigidbody playerRB;
    public float followSpeed;
    Vector3 distance;
    public float rotationSpeed=5.0f;

    public float speed;
    public Vector3 Offset;

    void Start()
    {
        //transform.position = player.transform.position;
        //distance = player.transform.position - transform.position;
        //playerRB = player.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void LateUpdate()
    {
        follow2();
    }
    private void FixedUpdate()
    {
        
    }

    void follow2()
    {
        if (!GameManager.stop)
        {
            Vector3 currentPos = player.position + Offset;
            //Vector3 carPos = player.transform.position - distance;
            transform.position = Vector3.Lerp(transform.position, player.position, followSpeed);
            //transform.position = Vector3.Lerp(currentPos, player.transform.position, followSpeed * Time.deltaTime);
            //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            //transform.position.
            /*if(SwipeInput.turn == SwipeInput.Turn.Left)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
            if (SwipeInput.turn == SwipeInput.Turn.Right)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }*/

            var rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, rotationSpeed);
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
        }
    }


    void follow1()
    {
        Vector3 playerForward = (playerRB.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position,
            player.position + player.transform.TransformVector(Offset)
            + playerForward * (-2f),
            speed * Time.deltaTime);

        transform.LookAt(player);
    }

}
