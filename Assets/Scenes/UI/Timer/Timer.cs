using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public eState m_nState;
    public float time = 300f;
    public TextMeshProUGUI display;
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
            case eState.timerStart:
                if (true)
                {
                    m_nState = eState.timerCount;
                }
                break;
            case eState.timerCount:
                display.faceColor = Color.white;
                time -= Time.deltaTime;
                UpdateTimerDisplay(time);
                if (Mathf.FloorToInt(time) < 60f)
                {
                    m_nState = eState.timerCrit;
                }
                break;
            case eState.timerCrit:
                display.faceColor = Color.red;
                Flash();
                time -= Time.deltaTime;
                UpdateTimerDisplay(time);
                if (Mathf.FloorToInt(time) <= 0f)
                {
                    m_nState = eState.timerStop;
                }
                break;
            case eState.timerStop:
                break;
            default:
                break;
        }
    }
    public void UpdateTimerDisplay(float time)
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
