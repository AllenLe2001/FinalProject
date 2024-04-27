using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBlast : MonoBehaviour
{
    //REFERENCE
    public Mage mage;
    public Transform player;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        
        rb = GetComponent<Rigidbody2D>();
        //calculate direction to player
        Vector2 blastDirect = (player.position - transform.position);
        //apply the force to move towards the target
        rb.velocity = blastDirect * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Debug.Log("Player hit");
             //Destroy(gameObject);
        }
        //destroy once collides
        Destroy(gameObject);
    }
}
