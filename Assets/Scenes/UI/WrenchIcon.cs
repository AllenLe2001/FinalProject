using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrenchIcon : MonoBehaviour
{
    public Image icon;
    public WrenchToss wrench;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        icon.enabled = wrench.hasWrench;
    }
}
