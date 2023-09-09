using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blockFalling : MonoBehaviour
{
    public GameObject[] blocks;
    public GameObject ramp;
    
    // Start is called before the first frame update

    void Start()
    {
      /*  int r = 0;//Random.Range(0, 2);
        if(r == 0)
        {
            ramp.SetActive(true);

        }
        else
        {
            blocks[2].SetActive(false);
        }*/
      }
    
    void enablePhysics() { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            foreach(GameObject block in blocks)
            {
                block.GetComponent<Rigidbody>().isKinematic = false;
                new WaitForSeconds(0.2f);
            }
           /* if (ramp.activeInHierarchy)
            {
                LeanTween.moveLocalX(blocks[1], -0.7f, 0.5f);
                LeanTween.moveLocalX(blocks[2], 0.2f, 0.5f);
                new WaitForSeconds(0.5f);
                blocks[2].GetComponent<Rigidbody>().isKinematic = false;
                blocks[1].GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                LeanTween.moveLocalX(blocks[1], -0.6f, 0.5f);
                new WaitForSeconds(0.5f);
                blocks[1].GetComponent<Rigidbody>().isKinematic = false;
            }*/

            //blocks[1].GetComponent<Rigidbody>().AddForce(Vector3.right);
            //blocks[2].GetComponent<Rigidbody>().AddForce(new Vector3(1,0,0));
           

        }
    }
}
