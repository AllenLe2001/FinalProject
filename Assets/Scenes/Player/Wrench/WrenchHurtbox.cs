 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchHurtbox : MonoBehaviour
{
    public int numBounce=1;
    public PhysicsMaterial2D bounce;
    public PhysicsMaterial2D noBounce;
    public eState hbState = eState.hbActive;
    private BoxCollider2D[] colliders;
    private int count = 0;
    private float activeTime;
    public enum eState : int
    {
        hbActive,
        hbBounce,
        hbInactive
    }

    void Start()
    {
        colliders = GetComponents<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (hbState)
        {
            case (eState.hbActive):
                {

                }
                break;

            case (eState.hbBounce):
                {
                    foreach(BoxCollider2D body in colliders)
                    {
                        body.sharedMaterial = noBounce;
                    }
                    hbState = eState.hbInactive;
                    activeTime = Time.time;
                }
                break;

            case (eState.hbInactive):
                {
                    if(activeTime+1.5f <= Time.time)
                    {
                        gameObject.tag = "inactive";
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
        count++;
        if (numBounce <= count &&hbState == eState.hbActive)
        {
            hbState = eState.hbBounce;
            count = 0;
        }
        
    }

}
