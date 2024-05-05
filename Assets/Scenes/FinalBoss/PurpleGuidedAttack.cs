using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleGuidedAttack : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;
    public float timer;
    public float attackDuration;
    public float explosionTimer;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        //calculate direction to player
        Vector2 blastDirect = (player.position - transform.position);
        //apply the force to move towards the target
        rb.velocity = blastDirect * 1.5f;
        if(timer >= attackDuration){
        anim.SetFloat("Speed", 1f , 0.1f, Time.deltaTime);
        explosionTimer += Time.deltaTime;
        if(explosionTimer >= 0.5f){
            Destroy(gameObject);
        }
        }
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Debug.Log("Player hit");
             
        }
    }
}