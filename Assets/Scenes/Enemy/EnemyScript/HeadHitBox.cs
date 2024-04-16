using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitBox : MonoBehaviour
{
    //reference to the knight enemy
    public KnightBehavior knight;
    public Transform knightRef;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
         //find the offset between the knight and the hitbox
        offset = transform.position - knightRef.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = knightRef.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D other){
        //if the knight is attacking and the hitbox collides with the player
            if(other.CompareTag("Player")){
                Debug.Log("Player jumped on top of knight");
                knight.isDead = true;
                //replace this later by referencing players health/life system
            }
     }
}
