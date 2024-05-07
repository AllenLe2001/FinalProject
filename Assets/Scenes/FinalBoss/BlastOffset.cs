using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastOffset : MonoBehaviour
{
   //reference to the boss
    public BossBehaviour boss;
    public Transform bossRef;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //find the offset between the boss and the hitbox
        offset = transform.position - bossRef.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = bossRef.position + offset;
    }
}
