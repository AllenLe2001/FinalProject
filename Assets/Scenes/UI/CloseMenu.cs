using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;
    public bool onOff = false;
    public float timestamp = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && Time.time >= timestamp)
        {
            onOff = !onOff;
            menu.SetActive(onOff);  
        }
    }
    public void turnOff()
    {
        if (!Input.GetKey(KeyCode.Escape))
            onOff = false;
        timestamp = Time.time + 0.2f;
    }
}
