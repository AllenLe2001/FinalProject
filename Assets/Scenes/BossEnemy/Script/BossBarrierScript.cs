using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrierScript : MonoBehaviour
{
     //REFERENCE
    //make a reference to our knight behavior script
    public BossKnightBehaviour boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "Boss"){
            Debug.Log("Boss has hit patrol area");
            //make the enemy knight the opposite direction once it has hit the barrier limit
            if(boss.moveDirection.x < 0){
                boss.moveDirection = Vector2.right;
            }

            else if(boss.moveDirection.x > 0){
                boss.moveDirection = Vector2.left;
            }

        }
    }
}
