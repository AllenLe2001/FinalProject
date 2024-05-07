using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidShrink : MonoBehaviour
{
    public bool shrink = true;
    public float scaleChange = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shrink)
        {
            if (transform.localScale.x - scaleChange * Time.deltaTime <= 0)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform.localScale -= new Vector3(scaleChange, scaleChange, 0);
            }
        }
        else
        {
            if (transform.localScale.x + scaleChange * Time.deltaTime >= 21)
            {
                transform.localScale = new Vector3(21,21,1);
            }
            else
            {
                transform.localScale += new Vector3(scaleChange, scaleChange, 0);
            }
        }

    }
}
