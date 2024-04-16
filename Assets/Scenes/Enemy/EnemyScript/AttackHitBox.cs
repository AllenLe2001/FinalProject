using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    //reference to the knight enemy
    public KnightBehavior knight;
    public Transform knightRef;
    public Vector3 offset;
    public Collider2D attackHitBox;
    public bool Attack;
    public bool playerHit;
    // Start is called before the first frame update
    void Start()
    {
        //find the offset between the knight and the hitbox
        offset = transform.position - knightRef.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(knight.isAttacking){
        attackHitBox.enabled = true;
        }
        else if (!knight.isAttacking){
           attackHitBox.enabled = false;
        }

        transform.position = knightRef.position + offset;
    }



     private void OnTriggerEnter2D(Collider2D other){
        //if the knight is attacking and the hitbox collides with the player
            if(other.CompareTag("Player")){
                Debug.Log("Player was hit by enemy");
                //replace this later by referencing players health/life system
            }
     }
}
