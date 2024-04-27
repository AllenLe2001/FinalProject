using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastPosition : MonoBehaviour
{
    //reference to the mage
    public Mage mage;
    public Transform mageRef;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //find the offset between the mage and the hitbox
        offset = transform.position - mageRef.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mageRef.position + offset;
    }
}
