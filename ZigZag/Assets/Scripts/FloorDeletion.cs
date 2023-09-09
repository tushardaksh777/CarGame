using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "player")
        {
            Invoke("deleteObject", 2.0f);
        }
    }
    void deleteObject()
    {

        var parent = gameObject.GetComponentInParent<Rigidbody>();
        parent.isKinematic = false;
        Destroy(parent.gameObject, 0f);
    }
}
