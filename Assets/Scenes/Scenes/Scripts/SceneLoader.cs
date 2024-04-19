using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public eState m_nState;
    public int lives;
    public string item;
    public int scene = 2;
    public float SplashDelay = 3f;
    private float delay = 0f;
    public enum eState : int
    {
        SceneStart,
        SceneLive,
        SceneIdle
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
                SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
                m_nState = eState.SceneLive;
                break;
            case eState.SceneLive:
                if(Time.time > delay)
                {
                    SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
                    m_nState = eState.SceneIdle;
                }
                
                break;
            case eState.SceneIdle:
                break;
            default:
                break;
        }
    }
    public void playGame()
    {
        delay = Time.time + SplashDelay;
        m_nState = eState.SceneStart;
    }
}
