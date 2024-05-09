using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    //REFERENCE
    //make a reference to our knight behavior script
    //public KnightBehavior knight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "KnightEnemy"){
            Debug.Log("Knight has hit patrol area");
            //make a reference to our knight behavior script
            KnightBehavior knight = other.GetComponent<KnightBehavior>();
            //make the enemy knight the opposite direction once it has hit the barrier limit
            if (knight.moveDirection.x < 0){
                knight.moveDirection = Vector2.right;
                knight.m_nState = 0;
                knight.moveSpeed = 1f;
            }

            else if(knight.moveDirection.x > 0){
                knight.moveDirection = Vector2.left;
                knight.m_nState = 0;
                knight.moveSpeed = 1f;
            }

        }
    }
}
