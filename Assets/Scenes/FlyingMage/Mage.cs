using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    
    //References 
    //reference to player to get their position
    public Transform Player; 
    public GameObject PlayerObject;
    public SpriteRenderer mageSprite;
    public GameObject mageObject;
    private Animator anim;
    public MageBlast blastPrefab;

    //Mage States
    public enum eState : int
    {
       mIdle,
       mChase,
       mAttack,
       mRetreat,
       mTeleport,
       mRegen,
       mDie,

    }

    public float attackSpeed = 2f;
    public float mageDistance;
    public float chaseDistance;
    public float stoppingDistance;
    public float retreatDistance;
    public float moveSpeed = 0.5f;
    public Vector2 moveDirection;
    public Vector2 chaseDirection;
    public float yConstrait;
    public float xConstrait;
    public float maxXConstrait;
    public float maxYConstrait;
    public float timer;
    public float teleportTime;
    public float teleportCharge;
    public float reviveTime;
    public Transform blastPosition;
    public Transform blastPosition2;
    public GameObject MageObject;
    //internal variables
    public eState m_nState;
    public bool recharge = false;
    public bool usedRevive = false;
    public bool isVulnerable = true;


    public AudioSource MageTeleport;
    public AudioSource ReviveSound;
    public AudioSource MageDeathSound;

    // Start is called before the first frame update
    void Start()
    {
         //initialize the enemy state to idle
        PlayerObject= GameObject.FindGameObjectWithTag("Player");
        Player = PlayerObject.transform;
        m_nState = eState.mIdle;
        mageSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //chase the player so we are moving towards the player
        chaseDirection = (Player.position - transform.position);
        chaseDirection = new Vector2(chaseDirection.x, transform.position.y);
        mageDistance = Vector2.Distance(transform.position, Player.position);
        switch (m_nState)
        {
            case eState.mIdle:
            {
                anim.SetFloat("Speed", 0 , 0.1f, Time.deltaTime);
                //if player is too close to the mage, it will try to retreat
                if(mageDistance < retreatDistance){
                    m_nState = eState.mRetreat;
                }
                //if the mage is in the chase range then it goes into chase state
                else if(mageDistance <= chaseDistance){
                   m_nState = eState.mChase;
                }
            }
            break;

            case eState.mChase:
            {
                anim.SetFloat("Speed", 0 , 0.1f, Time.deltaTime);
                //if the player is outside the chase distance then go back to idle
                if(mageDistance > chaseDistance){
                    m_nState = eState.mIdle;
                }
                //also check the movement direction so we can flip the sprite
                if(chaseDirection.x > 0){
                    mageSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    mageSprite.flipX = false;
                }
                if(mageDistance >= stoppingDistance){
                    //moving the mage up to the stopping distance
                     Vector2 newSpot = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);
                     transform.position = new Vector2(newSpot.x, Mathf.Clamp(newSpot.y, yConstrait, 999999f));
                }
                // mage will start to attack 
                else if (mageDistance < stoppingDistance){
                    m_nState = eState.mAttack;
                }
            }
            break;

            case eState.mAttack:
            {
                if(chaseDirection.x > 0){
                    mageSprite.flipX = true;
                }
                else if(chaseDirection.x < 0){
                    mageSprite.flipX = false;
                }
                anim.SetFloat("Speed", 0.25f, 0.1f, Time.deltaTime);
                timer += Time.deltaTime;
                if(timer >= attackSpeed && !recharge){
                    if(chaseDirection.x > 0){
                    Instantiate(blastPrefab, blastPosition2.position, transform.rotation);
                    timer = 0f;
                    recharge = true;
                    }
                    else if(chaseDirection.x < 0){
                    Instantiate(blastPrefab, blastPosition.position, transform.rotation);
                    timer = 0f;
                    recharge = true;
                    }
                }
                else if (recharge){
                //launches an attack and goes right back to idle
                     recharge = false;
                     m_nState = eState.mIdle;
                }
            }
            break;

            case eState.mRetreat:
            {
                //if the mage is within stopping range we go back to idle
                if(mageDistance > chaseDistance){
                    m_nState = eState.mIdle;
                    timer = 0f;
                }
                else if(timer >= teleportTime){
                    m_nState = eState.mTeleport;
                    timer = 0f;
                }
                timer += Time.deltaTime;
                //retreating state
                anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                Vector2 newSpot = transform.position;
                
                //if the mage is facing right so we want to go left
                 if(chaseDirection.x > 0){  
                    mageSprite.flipX = false;
                    newSpot = new Vector2(newSpot.x - 4f, newSpot.y);

                 }
                 else if (chaseDirection.x < 0){
                    mageSprite.flipX = true;
                    newSpot = new Vector2(newSpot.x + 4f, newSpot.y);
                 }
                newSpot = Vector2.MoveTowards(transform.position, newSpot, moveSpeed * Time.deltaTime);
                transform.position = new Vector2(newSpot.x, Mathf.Clamp(newSpot.y, yConstrait, 999999f));
            }
            break;

            case eState.mTeleport:
            {
                MageTeleport.Play();
                anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                timer += Time.deltaTime;
                //buffer time before the mage is able to teleport
                //aka when its most vulnerable
                if(timer >= teleportCharge){
                    anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
                    float randomDeterminer = Random.Range(0,1);
                    float randomXVal = 0;

                    if(randomDeterminer == 0){
                        randomXVal = -20;
                    }
                    else if(randomDeterminer == 1){
                        randomXVal = 20;
                    }
                    
                    Vector3 randomPosition = transform.position + new Vector3(randomXVal, 0, 0);
                    transform.position = randomPosition;
                    timer = 0;
                    m_nState = eState.mIdle;
                }
            }
            break;

            case eState.mRegen:
            {
                anim.SetFloat("Speed", 0.75f, 0.1f, Time.deltaTime);
                timer += Time.deltaTime;
                //once the animation ends we head right into idle state
                if(timer >= reviveTime){
                    timer = 0;
                    m_nState = eState.mIdle;
                    isVulnerable = true;
                }

            }
            break;

            case eState.mDie:
            {
            MageDeathSound.Play();
              timer += Time.deltaTime;
              anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);  
              float deathFrameDuration = 2f;
              if(timer >= deathFrameDuration){
                    //here we shut off the whole gameobject
                    MageObject.SetActive(false);
                }
            }
            break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other){
        //if the mage is touched by player it dies
            if(other.CompareTag("Player") || other.CompareTag("Wrench")){
                //mage has one revive so check if revive has been used yet
                if(!usedRevive && isVulnerable){
                    ReviveSound.Play();
                    //set the to not Vulenerable so we can't kill during reviving
                    isVulnerable = false;
                    usedRevive = true;
                    m_nState = eState.mRegen;
                }
                else if (usedRevive && isVulnerable){
                    Debug.Log("Player killed mage");
                    m_nState = eState.mDie;
                }
                
        
            }
     }
}
