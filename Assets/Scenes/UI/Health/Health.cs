using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour
{
    public TextMeshProUGUI display; //text gameobject
    private SceneLoader loader; //script of scene manager
    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
        loader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        display.text = "X " + loader.lives;  //display # of lives
    }

    // Update is called once per frame
    void Update()
    {

    }

}
