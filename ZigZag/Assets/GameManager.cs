using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    public GameObject baseSpawner;
    public static bool stop;

    public static bool carStopped = false;
    public static bool lights = false;
    

    public bool scoreCount;
    public int score=0;
    public Text scoreText;
    
    public Text diamondText;
    public int diamond=0;
    public bool diamondCount; 
    


    private void Awake()
    {
        if(instance == null)
        {
                instance = this;
        }
    }
    void Start()
    {
        Application.targetFrameRate = 120;
        //StartCoroutine(coinGen());
        //StartCoroutine(countScore());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Time.timeScale =0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Time.timeScale = 1;
        }
        if (stop )
        {
            stop = false;
            carStopped = false;
            SceneManager.LoadScene("SampleScene");
        }
       

    }



    //  IEnumerator coinGen(){
    //     while (!GameManager.stop)
    //     {
    //         if(GameManager.diamond){
    //         GameManager.diamond = false;
    //     }else{
    //         GameManager.diamond = true;
    //     }
    //      yield return new WaitForSeconds(5f);
    //     }

    //  }

    IEnumerator countScore(){
        while(!scoreCount){
         yield return  new WaitForSeconds(0.2f);
         score++;
         scoreText.text = score.ToString("D5");
        }
     }

    public int getDiamond(){
    int value = PlayerPrefs.GetInt("Diamond");
    return value;
    }

    public void setDiamond(int value){
        diamond = diamond +value;
        diamondText.text = diamond.ToString("D3");
    }

     
}
