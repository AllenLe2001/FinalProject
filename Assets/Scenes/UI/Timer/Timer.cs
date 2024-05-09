using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public eState m_nState; //current timer state
    public float time = 90f; //total time of timer
    public float criticalTime = 30f; //at what second should timer turn red
    public TextMeshProUGUI display;
    public GameObject player;
    public GameObject killorb;
    private GameObject playerKiller;
    public enum eState : int
    {
        timerStart,
        timerCount,
        timerCrit,
        timerStop,
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (m_nState)
        {
            case eState.timerStart://starts
                if (true)
                {
                    m_nState = eState.timerCount;
                }
                break;
            case eState.timerCount: //timer is counting down
                display.faceColor = Color.white;
                time -= Time.deltaTime;
                UpdateTimerDisplay(time);
                if (Mathf.FloorToInt(time) < criticalTime) //next state when timer is low
                {
                    m_nState = eState.timerCrit;
                }
                break;
            case eState.timerCrit://changes timer color
                display.faceColor = Color.red;
                Flash();
                time -= Time.deltaTime;
                UpdateTimerDisplay(time);
                if (Mathf.FloorToInt(time) <= 0f) //stops timer
                {
                    m_nState = eState.timerStop;
                    playerKiller = Instantiate(killorb);
                    killorb.transform.SetParent(player.transform);
                }
                break;
            case eState.timerStop:
                playerKiller.transform.position = player.transform.GetChild(0).transform.position;
                break;
            default:
                break;
        }
    }
    public void UpdateTimerDisplay(float time) //displays timer
    {
        float seconds = Mathf.FloorToInt(time);
        //float minutes = Mathf.FloorToInt(time / 60f);
        //float seconds = Mathf.FloorToInt(time % 60f);
        //string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        string currentTime = string.Format("{0:N0}", seconds);
        display.text = currentTime;
    }
    private void Flash()
    {

    }
}
