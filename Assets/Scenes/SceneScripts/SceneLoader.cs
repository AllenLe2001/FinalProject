using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public eState m_nState;  //current state of sceneloader
    public int lives = 3;   //number of lives the player has
    public int scene = 2;   //level number to display
    public float SplashDelay = 3f;  //duration of level splash creen
    private float delay = 0f;       //delay to start next state of sceneloader
    public int sceneCount = 4;     //current scene index found in build setting
    public enum eState : int
    {
        SceneStart,
        SceneLive,
        SceneIdle,
        SceneGameOver,
        SceneContinue
    }
    // Start is called before the first frame update
    void Awake()
    {   
        GameObject[] objs = GameObject.FindGameObjectsWithTag("scene manager");

        if (objs.Length > 1)
        {
            Destroy(objs[0]);
        }
        DontDestroyOnLoad(this.gameObject);
        m_nState = eState.SceneIdle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_nState)
        {
            case eState.SceneStart: //loads splash screen and sets level number
                scene = sceneCount-3;
                SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
                delay = Time.time + SplashDelay;
                m_nState = eState.SceneLive;
                break;
            case eState.SceneLive: //loads level

                if (Time.time > delay || Input.GetKey(KeyCode.Space))
                {
                    SceneManager.LoadSceneAsync(sceneCount, LoadSceneMode.Single);
                    sceneCount += 1;
                    m_nState = eState.SceneIdle;
                }
                break;
            case eState.SceneIdle: //empty state for when level is being played
                break;
            case eState.SceneGameOver: //loads gameover scene
                SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                lives = 3;
                delay = Time.time + SplashDelay;
                m_nState = eState.SceneContinue;
                break;
            case eState.SceneContinue:
                if (Time.time > delay+1f || Input.GetKey(KeyCode.Space))
                {
                    SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
                    sceneCount = 4;
                    m_nState = eState.SceneIdle;
                }
                break;
            default:
                break;
        }
    }

    public void died()  //used in playerhit script to start player death sequence
    {
        sceneCount -= 1;
        lives -= 1;
        if(lives < 0) //if out of lives go to gameover
        {
            m_nState = eState.SceneGameOver;
        }
        else //else reload level
        {
            m_nState = eState.SceneStart;
        }
        
    }
    public void playGame(int index) //for main menu use, loads level selected
    {
        sceneCount = index;
        delay = Time.time + SplashDelay;
        m_nState = eState.SceneStart;
    }
}
