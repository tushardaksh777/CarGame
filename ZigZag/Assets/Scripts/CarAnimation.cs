using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    public static CarAnimation instance;
    public bool stop = false;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        StartCoroutine(startshocksAnimation());

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void moveWheelsToRight()
    {
        animator.SetBool("left", false);
        animator.SetBool("right", true);
        animator.SetBool("shocks", false);
        stop= true;
    }

    public void moveWheelsToLeft()
    {
        animator.SetBool("left", true);
        animator.SetBool("right", false);
        animator.SetBool("shocks", false);
        stop = true;
    }
    public void moveWheelsToStraight()
    {
        animator.SetBool("left", false);
        animator.SetBool("right", false);
        stop = false;

        StartCoroutine(startshocksAnimation());
    }

    IEnumerator startshocksAnimation()
    {
        while (!stop)
        {
            float time = Random.RandomRange(0.25f, 3f);
            animator.SetBool("shocks", true);
            // new WaitForSeconds(time);

            yield return new WaitForSeconds(time);
            animator.SetBool("shocks", false);
            yield return new WaitForSeconds(time);
        }
        //animator.SetBool("left", false);
       // animator.SetBool();
       
    }

    public void stopCarShocksAnimation()
    {
        animator.SetBool("shocks", false);
    }
}
