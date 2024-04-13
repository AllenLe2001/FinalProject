using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehavior : MonoBehaviour
{
    //References 
    //reference to player to get their position
    public Transform Player; 
    public Transform PatrolBoundary1;
    public Transform PatrolBoundary2;
    public SpriteRenderer knightSprite;
    private Animator anim;

    //The knight has 3 states
    //Patrolling, Chasing, Attacking, and Death
    public enum eState : int
    {
        kPatrol,
        kChase,
        kAttack,
        kDie,
    }

    //external variables that we can tune
    public float attackDistance;
    public bool isInRange;
    public Vector3 endPatrolPos;
    public float moveSpeed = 0.5f;
    // intital movement for the enemy (this is -1 Left 1 is right)
    public Vector2 moveDirection = Vector2.left; 

    //internal variables
    public eState m_nState;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to patrolling
        m_nState = eState.kPatrol;
        endPatrolPos = PatrolBoundary2.position;
        knightSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //different states of the knight enemy
        switch (m_nState)
        {
            case eState.kPatrol:
            {
                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                //also check the movement direction so we can flip the sprite
                if(moveDirection == Vector2.right){
                    knightSprite.flipX = true;
                }
                else if(moveDirection == Vector2.left){
                    knightSprite.flipX = false;
                }
                //moving the knight 
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            break;
            case eState.kChase:
            {
            
            }
            break;
            case eState.kAttack:
            {

            }
            break;
            case eState.kDie:
            {

            }
            break;
        }

    }

}
