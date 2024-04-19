using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public eState m_nState;
    public int lives;
    public string item;
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_nState)
        {
            case eState.SceneStart:
                break;
            case eState.SceneLive:               
                break;
            case eState.SceneIdle:
                break;
            default:
                break;
        }
    }
}
