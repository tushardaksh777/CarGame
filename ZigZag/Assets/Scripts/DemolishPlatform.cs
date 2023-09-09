using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolishPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] row1;
    public GameObject[] row2;
    public GameObject[] row3;

    int rowsSelect;
    float delay = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "player")
        {
            int rowSelect = Random.Range(0, 3);
            // GameObject[] children= gameObject.get
            if (rowSelect == 0)
            {
                //first row and second row OR LeftRow and Center Row
                startFalling(row1);
                startFalling(row2);

            }
            else if (rowSelect == 1)
            {
                //Second row and Third row OR Center and Right Row
                startFalling(row2);
                startFalling(row3);
            }
            else
            {
                //first row and Third row OR Left Row and Right Row
                startFalling(row1);
                startFalling(row3);
            }
        }
    }

    void startFalling(GameObject[] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            StartCoroutine(fall(gameObjects[i] , i * 0.03f));
        }
    }

    void fallObject(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, 2f);
    }
    IEnumerator fall(GameObject gameO , float delay)
    {
        yield return new WaitForSeconds(delay);
        gameO.GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameO, 5f);
    }
}
