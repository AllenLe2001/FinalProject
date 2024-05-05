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
    public float phaseNum = 1f;
    public float AttackNum = 1f;
    public float randomTimer;
    public GameObject shield;

    public float bossDistance;
    public float moveSpeed = 0.5f;
    public float timer;
    public bool recharge;
    public float shieldDuration = 5f;
    public float shieldTimer;
    float randomValue = 99999f;
    public bool isVulnerable = true;

    public BasicBlastBehaviour BasicBlastPrefab;
    public PurpleGuidedAttack PurplePrefab;
    public Transform LaunchOffset;
    public Transform LaunchOffset1;

     //internal variables
    public eState m_nState;

    //boss States
    public enum eState : int
    {
       bIdle,
       bAttack,
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
        randomTimer += Time.deltaTime;
        //call upon a random float number to determine if boss can go have a shield
        if(randomTimer >= 5f){
            randomValue = Random.value;
            randomTimer = 0f;
        }
        shieldTimer += Time.deltaTime;
        
        //30 percent chance for shield to be on and invulnerable state
        if(randomValue <= 0.3f && isVulnerable){
            shield.SetActive(true);
            isVulnerable = false;
        }
        //after shield duration is over back to being vulnerable
        if(shieldTimer >= shieldDuration){
            shield.SetActive(false);
            isVulnerable = true;
            shieldTimer = 0f;
        }

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
                anim.SetFloat("Speed", 0.25f , 0.1f, Time.deltaTime);
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
                    //moving the boss up to the stopping distance
                     Vector2 newSpot = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
                     transform.position = newSpot;
                }
                // boss will start to attack 
                else if (bossDistance < stoppingDistance){
                    m_nState = eState.bAttack;
                }

            }
            break;

            case eState.bAttack:
            {
                AttackNum = Random.Range(1,3);
                 //also check the movement direction so we can flip the sprite
                if(chaseDirection.x > 0){
                    bossSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    bossSprite.flipX = false;
                }
                timer += Time.deltaTime;
                anim.SetFloat("Speed", 0.75f , 0.1f, Time.deltaTime);
                //in phase one the boss will have 3 attacks to use from
                if(phaseNum == 1){
                    if(timer >= 1.071f){
                        recharge = false;
                        timer = 0f;
                    }
                    Debug.Log("We are in phase 1");
                    //basic blast attack
                    if(AttackNum == 1){
                        if(timer >= 0.49f && !recharge){
                         if(chaseDirection.x < 0){
                            Instantiate(BasicBlastPrefab, LaunchOffset.position, transform.rotation);
                            recharge = true;
                            }
                        else if(chaseDirection.x > 0){
                            Instantiate(BasicBlastPrefab, LaunchOffset1.position, transform.rotation);
                            recharge = true;
                        }
                        }
                    }
                    //tracking guided attack
                    else if(AttackNum == 2){
                          if(timer >= 0.49f && !recharge){
                         if(chaseDirection.x < 0){
                            Instantiate(PurplePrefab, LaunchOffset.position, transform.rotation);
                            recharge = true;
                            }
                        else if(chaseDirection.x > 0){
                            Instantiate(PurplePrefab, LaunchOffset1.position, transform.rotation);
                            recharge = true;
                        }
                        }
                    }
                }
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
