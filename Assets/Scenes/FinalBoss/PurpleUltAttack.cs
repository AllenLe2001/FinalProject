using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleUltAttack : MonoBehaviour
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
        transform.position = bossRef.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        //find the offset between the boss and the hitbox
        offset = transform.position - bossRef.position;
        transform.position = bossRef.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(!other.CompareTag("Player")){
         Debug.Log("Player was OBLITERATED");
        }
    }

}
