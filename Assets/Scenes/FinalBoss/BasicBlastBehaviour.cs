using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlastBehaviour : MonoBehaviour
{
    public BossBehaviour boss;
    public GameObject bossRef;
    public float Speed = 20;
    public float Timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
       bossRef = GameObject.Find("PrincessBoss");
       boss =  bossRef.GetComponent<BossBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(boss.chaseDirection.x < 0){
        transform.position += -transform.right * Time.deltaTime * Speed;
        }
        else if(boss.chaseDirection.x > 0){
        transform.position += transform.right * Time.deltaTime * Speed;
        }
        //destroy the laser after a certain time
        if(Timer >=  5f){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
         Debug.Log("Player was hit");
         Destroy(gameObject);
        }
    }
}
