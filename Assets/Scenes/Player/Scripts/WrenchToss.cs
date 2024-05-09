using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchToss : MonoBehaviour
{
    public float power = 16f;
    public float charge = 0f;
    public float chargeTime = 0.35f;
    public GameObject wrench;
    public float Delay = 7f;
    private float activeTime;
    private bool hasWrench = true;
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
            case (eState.initToss):
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

            case (eState.chargeToss):
                {
                    charge = Mathf.Clamp(charge + Time.deltaTime * chargeTime, 0f, chargeTime);
                    if(!Input.GetKey("left shift"))
                    {
                        wrenchState = eState.releaseToss;
                    }
                    
                }
                break;

            case (eState.releaseToss):
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

            case (eState.pickupToss):
                {

                }
                break;

            default:
                break;
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "inactive"|| collision.gameObject.tag == "Wrench")
        {
            hasWrench = true;
            Destroy(collision.gameObject);
        }

    }
}
