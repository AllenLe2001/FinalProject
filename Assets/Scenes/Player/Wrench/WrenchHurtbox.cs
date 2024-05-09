 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchHurtbox : MonoBehaviour
{
    public AudioSource hitSound; //hit sound
    public int numBounce=1;//number of bounces before tuning down bounciness
    public PhysicsMaterial2D bounce; //bouncy material
    public PhysicsMaterial2D noBounce; //non bouncy material
    public eState hbState = eState.hbActive; //current wrench state
    private BoxCollider2D[] colliders; //list of colliders
    private int count = 0; //number of bounces so far
    private float activeTime; //timestamp of first bounce, used to determine active hitbox
    private float startTime; //timestamp of start of initialization
    private float soundDelay;
    public enum eState : int
    {
        hbActive,
        hbBounce,
        hbInactive
    }

    void Start()
    {
        colliders = GetComponents<BoxCollider2D>();
        startTime = Time.time + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime <= Time.time) //changes layer to avoid collision at start
        {
            gameObject.layer = 18;
        }
        switch (hbState)
        {
            case (eState.hbActive): //curently active
                {

                }
                break;

            case (eState.hbBounce): //counts bounces
                {
                    foreach(BoxCollider2D body in colliders)
                    {
                        body.sharedMaterial = noBounce;
                    }
                    hbState = eState.hbInactive;
                    activeTime = Time.time;
                }
                break;

            case (eState.hbInactive): //disables hurtbox against enemies
                {
                    
                    if (activeTime+1.5f <= Time.time)
                    {
                        gameObject.tag = "inactive";
                        gameObject.layer = 19;
                        //Destroy(gameObject);
                    }
                    if (startTime + 6f <= Time.time)
                    {
                        Destroy(gameObject);
                    }
                }
                break;

            default:
                break;
        }
    }
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if(soundDelay <= Time.time)
        {
            hitSound.Play();
            soundDelay = Time.time +0.5f;
        }
        count++;
        if (numBounce <= count &&hbState == eState.hbActive)//changes material after set number of bounces
        {
            hbState = eState.hbBounce;
            count = 0;
        }
        
    }

}
