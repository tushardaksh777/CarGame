using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI floorsize;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setTextForfloorsize();
    }

    void setTextForfloorsize()
    {
        floorsize.SetText("Floor Size - " + FloorGenration.instance.gameObject.transform.childCount);
    }
}
