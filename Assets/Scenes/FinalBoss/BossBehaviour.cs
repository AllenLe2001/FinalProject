using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //References 
    //reference to player to get their position
    public Transform Player; 
    public SpriteRenderer mageSprite;
    public GameObject bossObject;
    private Animator anim;

     //internal variables
    public eState m_nState;

    //Mage States
    public enum eState : int
    {
       bIdle,
       bAttack,
       bInvulnernable,
       bSpawn,
       bDie,

    }
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to idle
        m_nState = eState.bIdle;
        mageSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

         switch (m_nState)
        {
             case eState.bIdle:
            {
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
