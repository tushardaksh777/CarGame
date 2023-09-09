using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour
{
    // Start is called before the first frame update
    private Material dayNightSkybox;
    public float dayNightTransistionTime = 10f;
    public bool isDay = true;
    void Start()
    {
        GameManager.lights = !isDay;
        dayNightSkybox = RenderSettings.skybox;
        StartCoroutine(dayNightTransition());
        dayNightSkybox.SetFloat("_CubemapTransition", 1f);
        Debug.Log("set " + dayNightSkybox.GetFloat("_CubemapTransition"));
        Debug.Log("Value " + dayNightSkybox.GetFloat("_CubemapTransition"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator dayNightTransition()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.01f);
        //switching(0.05f);
         while (!GameManager.stop)
          {
            
            float dayNig = dayNightSkybox.GetFloat("_CubemapTransition");
            yield return new WaitForSeconds(dayNightTransistionTime);
            if (!isDay)
            {
                //day
                isDay = true;
                //RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.white , 5f);
                while (dayNig <= 1f)
                {

                    dayNig = dayNightSkybox.GetFloat("_CubemapTransition");
                    dayNightSkybox.SetFloat("_CubemapTransition", dayNig + 0.01f);
                    if(dayNig > 0.2f)
                    {
                        GameManager.lights = false;
                    }
                    RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, Color.white, dayNig);
                    /*if(dayNig > 0.5f)
                    {
                        dayNightSkybox.SetFloat("_RotationSpeed", dayNightSkybox.GetFloat("_RotationSpeed")+ (dayNig * 4f));
                    }*/
                    
                    //Debug.Log("set " + dayNightSkybox.GetFloat("_CubemapTransition") + "  " + dayNig);
                    yield return new WaitForSeconds(0.05f);
                    
                }
                

            }
            else
            {
                //night
                isDay = false;
                
                while (dayNig >= 0f)
                {
                    dayNig = dayNightSkybox.GetFloat("_CubemapTransition");
                    dayNightSkybox.SetFloat("_CubemapTransition", dayNig - 0.01f);
                    if (dayNig < 0.8f)
                    {
                        GameManager.lights = true;
                    }
                    RenderSettings.fogColor = Color.Lerp(Color.black , RenderSettings.fogColor , dayNig);
                 /*   if (dayNig < 0.5f)
                    {
                        dayNightSkybox.SetFloat("_RotationSpeed", dayNightSkybox.GetFloat("_RotationSpeed") - (0.01f * 4f));
                    } */
                    //Debug.Log("set " + dayNightSkybox.GetFloat("_CubemapTransition") + "  " + dayNig);
                    yield return new WaitForSeconds(0.05f);
                    
                }
                
            }

             //switching(time);

          } 
         
        

        /*float time = (4 / 10);
        if (isDay)
        {
            //Night
            for (int i = 1; i <= 100; i++)
            {
                //float time = (2 / 10);
                yield return new WaitForSeconds(time);
                dayNightSkybox.SetInt("_CubemapTransition", i / 100);
            }
            isDay = false;


        }
        else
        {
            //day
            for (int i = 100; i >= 0; i--)
            {

                yield return new WaitForSeconds(time);
                dayNightSkybox.SetInt("_CubemapTransition", i / 100);
                isDay = true;
            }
        }*/



        
    }

   void switching(float time)
    {
        if (!isDay)
        {
            //Night
            for (float i = 1; i <= 10f; i++)
            {
                //float time = (2 / 10);
               new WaitForSeconds(time);
                dayNightSkybox.SetFloat("_CubemapTransition", i / 10f);
                Debug.Log("Night " + dayNightSkybox.GetFloat("_CubemapTransition"));
            }
            isDay = false;


        }
        else
        {
            //day
            for (float i = 10; i >= 0; i--)
            {

                 new WaitForSeconds(time);
                dayNightSkybox.SetFloat("_CubemapTransition", i / 10f);
                Debug.Log("Day " + dayNightSkybox.GetFloat("_CubemapTransition"));
                isDay = true;
            }
        }
    }
}
