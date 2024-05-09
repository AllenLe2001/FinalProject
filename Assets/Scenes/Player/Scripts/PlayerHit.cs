using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHit : MonoBehaviour
{
    public GameObject blood;
    public AudioSource deathSound; //death sound
    private SceneLoader loader; //scene manager script
    public float deathDelay = 2f; //how long to show death
    private float timer = 0f; //time of death
    private bool dead = false;//check if player is dead
    public float speed = 1f; //speed of rotation
    // Start is called before the first frame update
    private bool locked = false; //lock out function

    void Start()
    {
        blood.SetActive(false);
        loader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemyAttack"&&!dead)  //checks if player is hit with an attack they die.
        {
            //add sound here
            deathSound.Play(); //play death sound
            blood.SetActive(true);
            dead = true;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-35, 0, -90), speed); //rotates sprite
            timer = Time.time + deathDelay;
            GetComponent<PlayerController>().enabled = false; //disables scripts
            GetComponent<WrenchToss>().enabled = false; //disables scripts
            
        }
    }
    public void Update()
    {
        if(dead && Time.time >= timer&&!locked) //if time elapses after death restart level
        {
            loader.died();
            locked = true;
        }

    }
}
