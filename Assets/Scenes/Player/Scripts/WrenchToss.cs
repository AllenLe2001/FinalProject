using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchToss : MonoBehaviour
{
    public float power = 16f; //strength of toss
    private float charge = 0f; //current charge, used to determine high or far toss
    public float chargeTime = 0.35f; //threshold for high throw
    public GameObject wrench; //wrench prefab
    public float Delay = 7f; //wrench respawn timer
    private float activeTime; //timestamp for respawn
    private bool hasWrench = true; //bool if player has wrench
    public eState wrenchState = eState.initToss;
    public enum eState : int
    {
        initToss,   
        chargeToss,      
        releaseToss,  
        pickupToss
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (wrenchState)
        {
            case (eState.initToss): //check if toss can be initiated
                {
                    if (Input.GetKey("left shift")&&activeTime<= Time.time)
                    {
                        hasWrench = true;
                        charge = 0f;
                        wrenchState = eState.chargeToss;
                    }
                    else if (Input.GetKey("left shift") && hasWrench)
                    {
                        charge = 0f;
                        wrenchState = eState.chargeToss;
                    }
                }
                break;

            case (eState.chargeToss): //checks for how long player hold button for
                {
                    charge = Mathf.Clamp(charge + Time.deltaTime * chargeTime, 0f, chargeTime);
                    if(!Input.GetKey("left shift"))
                    {
                        wrenchState = eState.releaseToss;
                    }                    
                }
                break;

            case (eState.releaseToss): //spawns wrench and adds initial speed
                {
                    float dir = transform.localScale.x / Mathf.Abs(transform.localScale.x);
                    GameObject allen = Object.Instantiate(wrench);
                    allen.transform.position = transform.position;
                    if(charge < chargeTime/2f)
                    {
                        allen.GetComponent<Rigidbody2D>().velocity = new Vector2(1f * dir, 0.25f).normalized * power;
                    }
                    else
                    {
                        allen.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * dir, 1.6f).normalized * 0.75f*power;
                    }
                    
                    
                    wrenchState = eState.initToss;
                    hasWrench = false;
                    activeTime = Time.time + Delay;
                }
                break;

            case (eState.pickupToss): //possible animation state
                {

                }
                break;

            default:
                break;
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "inactive"|| collision.gameObject.tag == "Wrench")//if colides with wrench despawns wrench and readies next toss
        {
            hasWrench = true;
            Destroy(collision.gameObject);
        }

    }
}
