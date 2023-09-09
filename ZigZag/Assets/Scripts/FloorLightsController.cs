using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLightsController : MonoBehaviour
{
    public GameObject lamp1;
    public GameObject lamp2;

    public GameObject lamp1Light;
    public GameObject lamp2Light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.lights)
        {
            if (!lamp1.activeInHierarchy)
            {
                set(true);
            }
        }
        else
        {
            if (!GameManager.lights)
            {
                if (lamp1.activeInHierarchy)
                {
                    set(false);
                }
            }
        }
    }

    void set(bool status)
    {
        lamp1.SetActive(status);
        lamp2.SetActive(status);
        lamp1Light.SetActive(status);
        lamp2Light.SetActive(status);

    }
}
