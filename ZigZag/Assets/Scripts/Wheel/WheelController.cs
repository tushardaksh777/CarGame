using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WheelController : MonoBehaviour
{

    public GameObject[] wheels;
    public float rotationSpeed;
    public Animator animator;
    public TrailRenderer[] trails;

    public static WheelController instance = null;
    // Start is called before the first frame update
    void Start()
    {
        
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
        var verticalInput = Input.GetAxisRaw("Vertical");
        var HorizontalInput = Input.GetAxisRaw("Horizontal");

        foreach(var wheel in wheels) {
            wheel.transform.Rotate(Time.deltaTime * verticalInput * rotationSpeed, 0, 0, Space.Self);
              
        }

        if(HorizontalInput > 0)
        {
            moveWheelsToRight();
        }else if(HorizontalInput < 0)
        {
            moveWheelsToLeft();
        }
        else
        {
            moveWheelsToStright();
        }

        if(HorizontalInput != 0)
        {
            startTrail();
        }
        else
        {
            stopTrail();
        }
    }
    public void startTrail()
    {
        foreach (var trail in trails)
        {
            trail.emitting = true;
        }
    }
    public void stopTrail()
    {
        foreach (var trail in trails)
        {
            trail.emitting = false;
        }
    }

    public void moveWheelsToRight() {
        animator.SetBool("left", false);
        animator.SetBool("right", true);
    }

    public void moveWheelsToLeft() {
        animator.SetBool("left", true);
        animator.SetBool("right", false);
    }
    public void moveWheelsToStright()
    {
        animator.SetBool("left", false);
        animator.SetBool("right", false);
    }
}
