using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnightBehaviour : MonoBehaviour
{
   //References 
    //reference to player to get their position
    public Transform Player; 
    public Transform PatrolBoundary1;
    public Transform PatrolBoundary2;
    public SpriteRenderer bossSprite;
    public GameObject bossObject;
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
    public float bossDistance;
    public float attackDistance;
    public bool isInRange;
    public float moveSpeed = 0.5f;
    public float chasePCoordX;
    public float chaseNCoordX;
    public float chaseDistance = 2f;
    public float playerXcoord;
    public bool isAttacking;
    public bool isDead;
    public float timer = 0f;
    public Vector2 moveDirection;
    public Vector2 chaseDirection;

    //internal variables
    public eState m_nState;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to patrolling
        m_nState = eState.kPatrol;
        bossSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead){
            m_nState = eState.kDie;
        }
        playerXcoord = Player.position.x;
        //different states of the knight enemy
        switch (m_nState)
        {
            case eState.kPatrol:
            {
                isAttacking = false;
                bossDistance = Vector2.Distance(transform.position, Player.position);
                //check if the distance from player and knight is close enough
                //if it is then we set to chase state
                if(bossDistance <= chaseDistance){
                        m_nState = eState.kChase;
                  }
                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                //also check the movement direction so we can flip the sprite
                if(moveDirection.x > 0){
                    bossSprite.flipX = true;
                }
                else if(moveDirection.x < 0){
                    bossSprite.flipX = false;
                }
                //moving the knight 
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            break;
            case eState.kChase:
            {
                isAttacking = false;
                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                bossDistance = Vector2.Distance(transform.position, Player.position);
                Debug.Log("Chasing Player now");
                //if the player is int between the barrier to be chased (in range)
                if(Player.position.x < chaseNCoordX || Player.position.x > chasePCoordX){
                    Debug.Log("Ignoring player");
                    m_nState = eState.kPatrol;
                    moveSpeed = 1f;
                }
                //check if the player is close enough for attack range 
                if(bossDistance <= attackDistance){
                    m_nState = eState.kAttack;
                }
                //chase the player so we are moving towards the player
                chaseDirection = (Player.position - transform.position);
                chaseDirection = new Vector2(chaseDirection.x, transform.position.y);
                //also check the movement direction so we can flip the sprite
                if(chaseDirection.x > 0){
                    bossSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    bossSprite.flipX = false;
                }
                //moving the knight 
                transform.Translate(chaseDirection * moveSpeed * Time.deltaTime);

                
            }
            break;
            case eState.kAttack:
            {
                
                bossDistance = Vector2.Distance(transform.position, Player.position);
                if(bossDistance > attackDistance){
                    m_nState = eState.kChase;
                    isAttacking = false;
                }
                isAttacking = true;
                Debug.Log("Attacking Now");
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            

            }
            break;
            case eState.kDie:
            {
                float deathFrameDuration = 0.5f;
                timer += Time.deltaTime;
               // anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
                if(timer >= deathFrameDuration){
                    //here we shut off the whole gameobject
                    bossObject.SetActive(false);
                }
                
            }
            break;
        }

    }

}
