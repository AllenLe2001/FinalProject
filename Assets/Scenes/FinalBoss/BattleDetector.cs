using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        //if player is detected then bossBattle is activated
            if(other.CompareTag("Player")){
                
                
        
            }
     }
}
