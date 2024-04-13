using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    //REFERENCE
    //make a reference to our knight behavior script
    public KnightBehavior knight;
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
            //make the enemy knight the opposite direction once it has hit the barrier limit
            if(knight.moveDirection == Vector2.left){
                knight.moveDirection = Vector2.right;
            }

            else if(knight.moveDirection == Vector2.right){
                knight.moveDirection = Vector2.left;
            }

        }
    }
}
