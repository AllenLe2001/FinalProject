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
    public float knightDistance;
    public float attackDistance;
    public bool isInRange;
    public float moveSpeed = 0.5f;
    public float chasePCoordX;
    public float chaseNCoordX;
    public float chaseDistance = 2f;
    public float playerXcoord;
    // intital movement for the enemy (this is -1 Left 1 is right)
    public Vector2 moveDirection;

    //internal variables
    public eState m_nState;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to patrolling
        m_nState = eState.kPatrol;
        knightSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerXcoord = Player.position.x;
        //different states of the knight enemy
        switch (m_nState)
        {
            case eState.kPatrol:
            {
                knightDistance = Vector2.Distance(transform.position, Player.position);
                //check if the distance from player and knight is close enough
                //if it is then we set to chase state
                if(knightDistance <= chaseDistance){
                        m_nState = eState.kChase;
                  }
                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                //also check the movement direction so we can flip the sprite
                if(moveDirection.x > 0){
                    knightSprite.flipX = true;
                }
                else if(moveDirection.x < 0){
                    knightSprite.flipX = false;
                }
                //moving the knight 
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            break;
            case eState.kChase:
            {

                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                knightDistance = Vector2.Distance(transform.position, Player.position);
                Debug.Log("Chasing Player now");
                //if the player is int between the barrier to be chased (in range)
                if(Player.position.x < chaseNCoordX || Player.position.x > chasePCoordX){
                    Debug.Log("Ignoring player");
                    m_nState = eState.kPatrol;
                    moveSpeed = 1f;
                }
                //check if the player is close enough for attack range 
                if(knightDistance <= attackDistance){
                    m_nState = eState.kAttack;
                }
                //chase the player so we are moving towards the player
                moveDirection = (Player.position - transform.position);
                moveDirection = new Vector2(moveDirection.x, transform.position.y);
                //also check the movement direction so we can flip the sprite
                if(moveDirection.x > 0){
                    knightSprite.flipX = true;
                }
                else if(moveDirection.x < 0){
                    knightSprite.flipX = false;
                }
                //moving the knight 
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                
            }
            break;
            case eState.kAttack:
            {
                
                knightDistance = Vector2.Distance(transform.position, Player.position);
                if(knightDistance > attackDistance){
                    m_nState = eState.kChase;
                }
                Debug.Log("Attacking Now");
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            

            }
            break;
            case eState.kDie:
            {

            }
            break;
        }

    }

}
