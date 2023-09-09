using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    public static CameraAnimation instance = null;
    public bool hasDriftStarted = false; 
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startRightDriftAni()
    {
        if (!hasDriftStarted)
        {
            LeanTween.moveLocal(gameObject, new Vector3(-1.5f, 0.7f, -0.9f), 0.25f).setOnComplete(() =>
            {
                
            });
            LeanTween.rotateLocal(gameObject, new Vector3(10.3f, 56f, -3.1f), 0.25f);
            hasDriftStarted = true;
        }
       

    }
    public void startLeftDriftAni()
    {
        if (!hasDriftStarted)
        {
            LeanTween.moveLocal(gameObject, new Vector3(1.5f, 0.7f, -0.9f), 0.25f).setOnComplete(() =>
            {

            });
            LeanTween.rotateLocal(gameObject, new Vector3(10.3f, -56f, -3.1f), 0.25f);
            hasDriftStarted = true;
        }


    }
    public void backToNormalCameraAnimation() 
    {
        if (hasDriftStarted)
        {
            LeanTween.moveLocal(gameObject, new Vector3(0, 1.4f, -2.3f), 0.25f);
            LeanTween.rotateLocal(gameObject, new Vector3(9.0f, 0.1f, -0.6f), 0.25f);
            hasDriftStarted = false;
            
        }
       
    }

    public void startBridgeAnimation()
    {
        LeanTween.moveLocal(gameObject, new Vector3(-0.33f, 0.9f, -1.8f), 0.5f);
        LeanTween.rotateLocal(gameObject, new Vector3(9.093f, 3.645f, -0.659f), 0.5f);
    }
    public void stopBridgeAnimation()
    {
        LeanTween.moveLocal(gameObject, new Vector3(0, 1.4f, -2.3f), 0.25f);
        LeanTween.rotateLocal(gameObject, new Vector3(9.0f, 0.1f, -0.6f), 0.25f);
    }

}
