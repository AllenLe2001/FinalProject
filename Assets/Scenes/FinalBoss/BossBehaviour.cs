using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //References 
    //reference to player to get their position
    public Transform Player; 
    public SpriteRenderer bossSprite;
    public GameObject bossObject;
    private Animator anim;
    public Vector2 chaseDirection;
    public float stoppingDistance;
    public float chaseDistance;
    public bool BattleStart = false;
    public Vector2 prevPos;
    public Rigidbody2D rb;
    public bool isMoving;

    public float bossDistance;
    public float moveSpeed = 0.5f;

     //internal variables
    public eState m_nState;

    //boss States
    public enum eState : int
    {
       bIdle,
       bAttack,
       bInvulnernable,
       bChase,
       bSpawn,
       bDie,

    }
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to idle
        m_nState = eState.bIdle;
        bossSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //chase the player so we are moving towards the player
        chaseDirection = (Player.position - transform.position);
        chaseDirection = new Vector2(chaseDirection.x, transform.position.y);
        bossDistance = Vector2.Distance(transform.position, Player.position);

        //check if boss is moving this is needed for the chase animations
        Vector2 currentPosition = (Vector2)transform.position;
        if(currentPosition != prevPos){
            isMoving = true;
            prevPos = currentPosition;
        }
        else if(currentPosition == prevPos){
            isMoving = false;
        }

         switch (m_nState)
        {
             case eState.bIdle:
            {
                if(bossDistance <= chaseDistance){
                    m_nState = eState.bChase;
                }
            }
            break;

            //basic chase state
            case eState.bChase:
            {
                //if we are moving set the moving animation else we just set to idle
                if(isMoving){
                anim.SetFloat("Speed", 0.5f , 0.1f, Time.deltaTime);
                }
                else if(!isMoving){
                anim.SetFloat("Speed", 0f , 0.1f, Time.deltaTime);  
                } 
                 //also check the movement direction so we can flip the sprite
                if(chaseDirection.x > 0){
                    bossSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    bossSprite.flipX = false;
                }
                if(bossDistance >= stoppingDistance){
                    //moving the mage up to the stopping distance
                     Vector2 newSpot = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
                     transform.position = newSpot;
                }
                // mage will start to attack 
                //else if (bossDistance < stoppingDistance){
                   // m_nState = eState.bAttack;
               // }

            }
            break;

            case eState.bAttack:
            {
            }
            break;

            case eState.bInvulnernable:
            {
            }
            break;

            case eState.bSpawn:
            {
            }
            break;

            case eState.bDie:
            {
            }
            break;


        }

        
    }
}
