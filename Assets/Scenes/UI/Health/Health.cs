using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour
{
    public TextMeshProUGUI display;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth(3);
    }

    void UpdateHealth(int hp)
    {
        string currentHealth = string.Format("{0:N0}", hp);
        display.text = "Health: "+currentHealth;
    }
}
