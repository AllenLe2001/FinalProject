using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private SceneLoader loader; //scene manager script
    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")              //if player collides with portal load next level
            loader.m_nState = SceneLoader.eState.SceneStart; 
    }
}
