using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private SceneLoader loader;
    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemyAttack")
            loader.died();
    }

}
