using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum camera{drift, main }
public class SwitchCamera : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject driftcamera;
    public GameObject mainCamera;

    public static SwitchCamera instance = null; 

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

    void deactivateAll()
    {
        driftcamera.SetActive(false); 
        mainCamera.SetActive(false);  
    }

    void switchCamra(camera cam)
    {
        if(cam == global::camera.drift)
        {
            deactivateAll();
            driftcamera.SetActive(true);
        }
        else
        {
            deactivateAll();
            mainCamera.SetActive(true);
        }
    }


}
