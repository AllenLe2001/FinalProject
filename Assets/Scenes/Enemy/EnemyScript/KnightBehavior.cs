using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //References 
    //reference to player to get their position
    public Transform Player; 
    public Transform PatrolBoundary1;
    public Transform PatrolBoundary2;
    public SpriteRenderer knightSprite;

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

    //internal variables
    public eState m_nState;
    // Start is called before the first frame update
    void Start()
    {
        //initialize the enemy state to patrolling
        m_nState = eState.kPatrol;
        endPatrolPos = PatrolBoundary2.position;
        knightSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //different states of the knight enemy
        switch (m_nState)
        {
            case eState.kPatrol:
            {
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
