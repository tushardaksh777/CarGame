using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharksAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;

    void Start()
    {
       //animator.SetBool("start", false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            animator.SetBool("start", true);
        }
    }
}
