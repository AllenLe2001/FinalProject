using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public eState m_nState;
    public int lives = 3;
    public string item;
    public int scene = 2;
    public float SplashDelay = 3f;
    private float delay = 0f;
    private int sceneCount = 2;
    public enum eState : int
    {
        SceneStart,
        SceneLive,
        SceneIdle,
        SceneGameOver
    }
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        m_nState = eState.SceneIdle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_nState)
        {
            case eState.SceneStart:
                scene = sceneCount-1;
                SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
                delay = Time.time + SplashDelay;
                m_nState = eState.SceneLive;
                break;
            case eState.SceneLive:
                if(Time.time > delay)
                {
                    SceneManager.LoadSceneAsync(sceneCount, LoadSceneMode.Single);
                    sceneCount += 1;
                    m_nState = eState.SceneIdle;
                }
                break;
            case eState.SceneIdle:
                break;
            case eState.SceneGameOver:
                SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
                lives = 3;
                m_nState = eState.SceneIdle;
                break;
            default:
                break;
        }
    }

    public void died()
    {
        sceneCount -= 1;
        lives -= 1;
        if(lives < 0)
        {
            m_nState = eState.SceneGameOver;
        }
        else
        {
            m_nState = eState.SceneStart;
        }
        
    }
    public void playGame()
    {
        delay = Time.time + SplashDelay;
        m_nState = eState.SceneStart;
        
    }
}
