using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    
    //References 
    //reference to player to get their position
    public Transform Player; 
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
    public float timer;
    public Transform blastPosition;
    public Transform blastPosition2;
    //internal variables
    public eState m_nState;
    public bool recharge = false;

    // Start is called before the first frame update
    void Start()
    {
         //initialize the enemy state to idle
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
                //if the mage is in the chase range then it goes into chase state
                if(mageDistance <= chaseDistance){
                   m_nState = eState.mChase;
                }
                else if(mageDistance < stoppingDistance){
                    m_nState = eState.mRetreat;
                }
            }
            break;

            case eState.mChase:
            {
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
                //launches an attack and goes right back to idle
                anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
                timer += Time.deltaTime;
                if(timer >= attackSpeed){
                    if(chaseDirection.x > 0){
                    Instantiate(blastPrefab, blastPosition2.position, transform.rotation);
                    }
                    else if(chaseDirection.x < 0){
                    Instantiate(blastPrefab, blastPosition.position, transform.rotation);
                    }
                    timer = 0f;
                    m_nState = eState.mIdle;
                  
                }
            }
            break;

            case eState.mRetreat:
            {
            }
            break;

            case eState.mTeleport:
            {
            }
            break;

            case eState.mRegen:
            {
            }
            break;

            case eState.mDie:
            {
            }
            break;
        }
    }
}
