using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlastBehaviour : MonoBehaviour
{
    public BossBehaviour boss;
    public GameObject bossRef;
    public float Speed = 30;
    // Start is called before the first frame update
    void Start()
    {
       bossRef = GameObject.Find("PrincessBoss");
       boss =  bossRef.GetComponent<BossBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boss.chaseDirection.x < 0){
        transform.position += -transform.right * Time.deltaTime * Speed;
        }
        else if(boss.chaseDirection.x > 0){
        transform.position += transform.right * Time.deltaTime * Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with OBJ");
        Destroy(gameObject);
    }
}
