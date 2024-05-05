using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOffset : MonoBehaviour
{
   //reference to the boss
    public BossBehaviour boss;
    public Transform bossRef;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - bossRef.position;
        transform.position = bossRef.position + offset;
    
    }

    // Update is called once per frame
    void Update()
    {
        offset = transform.position - bossRef.position;
        transform.position = bossRef.position + offset;
    }
}
