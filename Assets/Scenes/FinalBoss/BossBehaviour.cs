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
    public float randomTimer2;
    public GameObject shield;

    public float bossDistance;
    public float moveSpeed = 0.5f;
    public float timer;
    public bool recharge;
    public float shieldDuration = 5f;
    public float shieldTimer;
    float randomValue = 99999f;
    float randomValue2 = 99999f;
    public bool isVulnerable = true;
    public bool ultUsed = false;
    public float ultTimer = 0;
    public float ultRecoveryTime;
    public float recoverTimer;
    public bool recoveryFromUlt = false;
    public bool ultWarning = false;
    public bool ultSpawned = false;
    public bool usedRevive = false;
    public float reviveTimer = 0;
    private SceneLoader loader;

    public BasicBlastBehaviour BasicBlastPrefab;
    public PurpleGuidedAttack PurplePrefab;
    public Transform LaunchOffset;
    public Transform LaunchOffset1;
    public GameObject ultOffset;
    public GameObject ultOffset1;
    public GameObject ultWarner;
    public GameObject ultWarner1;
    public PurpleUltAttack ultPrefab;
    public Mage MagePrefab;
    public AudioSource reviveSound;
    public GameObject portal;

     //internal variables
    public eState m_nState;

    //boss States
    public enum eState : int
    {
       bIdle,
       bAttack,
       bChase,
       bRevive,
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
        loader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
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
        
        if(phaseNum == 1){
        //30 percent chance for shield to be on and invulnerable state
        if(randomValue <= 0.3f && isVulnerable){
            shield.SetActive(true);
            isVulnerable = false;
        }
        }
        else if(phaseNum == 2){
            //90 percent chance for shield to be on and invulnerable state
        if(randomValue <= 0.9f && isVulnerable){
            shield.SetActive(true);
            isVulnerable = false;
        }
        }
        //after shield duration is over back to being vulnerable
        if(shieldTimer >= shieldDuration){
            shield.SetActive(false);
            isVulnerable = true;
            shieldTimer = 0f;
            randomValue = 9999f; //reset the value
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
                anim.SetFloat("Speed", 0f , 0.1f, Time.deltaTime); 
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
                anim.SetFloat("Speed", 0.2f , 0.1f, Time.deltaTime);
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
                //the boss has to be in range
                if(bossDistance <= stoppingDistance){
                randomTimer2 += Time.deltaTime;
                //only use this when the ult hasn't been used
                if(!recoveryFromUlt){
                if(!ultUsed){
                AttackNum = Random.Range(1,3);
                }
                 //also check the movement direction so we can flip the sprite
                
                //limit the change in direction of boss when ulting
                if(!ultUsed){
                if(chaseDirection.x > 0){
                    bossSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    bossSprite.flipX = false;
                }
                }
                if(!ultUsed){
                timer += Time.deltaTime;
                }
                anim.SetFloat("Speed", 0.6f , 0.1f, Time.deltaTime);
                }
                else if(recoveryFromUlt){
                    anim.SetFloat("Speed", 0f , 0.1f, Time.deltaTime);
                    //if we are recovering ult
                    recoverTimer += Time.deltaTime;
                    if(recoverTimer >= 1.5f){
                        recoveryFromUlt = false;
                        recoverTimer = 0f;
                    }
                }
                //in phase one the boss will have 3 attacks to use from
                    if(timer >= 1.071f){
                        recharge = false;
                        timer = 0f;
                    }
                    Debug.Log("We are in phase 1");

                    if(randomTimer2 >= 15f){
                        randomValue2 = Random.value;
                         //50 percent chance for boss to use ult in phase 1
                         if(randomValue2 <= 0.5f){
                             AttackNum = 3f;
                             recharge = false;
                        }
                    }
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
                    //big ult attack
                    else if(AttackNum == 3){
                        ultTimer += Time.deltaTime;
                        anim.SetFloat("Speed", 0.8f , 0.1f, Time.deltaTime);
                        ultUsed = true;
                        if(ultTimer >= 0.21 && !ultWarning){
                            ultWarning = true;
                            if(!bossSprite.flipX){
                                ultWarner.SetActive(true);
                            }
                            else if(bossSprite.flipX){
                                ultWarner1.SetActive(true);
                            }
                        }
                        if(ultTimer >= 1.28 && !recharge){
                            if(!ultSpawned){
                            if(!bossSprite.flipX){
                                ultWarner.SetActive(false);
                                ultSpawned = true;
                                ultOffset.SetActive(true);
                            }
                            else if(bossSprite.flipX){
                                ultSpawned = true;
                                ultWarner1.SetActive(false);
                                ultOffset1.SetActive(true);
                            }
                            }
                            //launch the crazy beam attack
                            recharge = true;
                        }
                        else if(ultTimer >= 1.92 && recharge){
                            ultOffset.SetActive(false);
                            ultOffset1.SetActive(false);
                            ultSpawned = false;
                            ultWarning = false;
                            ultUsed = false;
                            ultTimer = 0;
                            AttackNum = 0f;
                            recoveryFromUlt = true;
                            randomTimer2 = 0;
                        }
                }
                }
                else if (bossDistance > stoppingDistance){
                    m_nState = eState.bIdle;
                }
            }
            break;
            case eState.bRevive:
            {
                anim.SetFloat("Speed", 0.4f , 0.1f, Time.deltaTime);
                reviveTimer += Time.deltaTime;
                if(reviveTimer >= 5f){
                isVulnerable = true;
                m_nState = eState.bIdle;
                phaseNum = 2;
                reviveTimer = 0f;
                }
            }
            break;


            case eState.bDie:
            {
                anim.SetFloat("Speed", 0.4f , 0.1f, Time.deltaTime);
                Debug.Log("Boss has been defeated");
                reviveTimer += Time.deltaTime;
                if(reviveTimer >= 5f)
                {
                    loader.skipSplash = true;
                    GameObject port = Instantiate(portal);
                    port.transform.position = new Vector3(32, 27, 0);
                    Destroy(gameObject);
                }
            }
            break;

        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        //if the mage is touched by player it dies
            if(other.CompareTag("Wrench")){
                //mage has one revive so check if revive has been used yet
                if(!usedRevive && isVulnerable){
                    reviveSound.Play();
                    //set the to not Vulenerable so we can't kill during reviving
                    isVulnerable = false;
                    usedRevive = true;
                    m_nState = eState.bRevive;
                    Debug.Log("Revive Used");
                }
                else if (usedRevive && isVulnerable){
                    reviveSound.Play();
                    Debug.Log("Player killed boss");
                    m_nState = eState.bDie;
                }
                
        
            }
     }
        
}
