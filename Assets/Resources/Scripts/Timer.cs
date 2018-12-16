using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Timer : MonoBehaviour {

    private bool leftTriggerDown = false;
    private bool rightTriggerDown = false;
    public bool m_gameStart = false;

    private Actor actor;
    private int timeLeft = 120;
    public Text countdownText;
    
    public Hand hand_left;
    public Hand hand_right;
    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (actor == null)
            actor = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Actor>();

        if (actor != null && !actor.leapStatus)
        { 
            leftTriggerDown = getPinchLeft();
            rightTriggerDown = getPinchRight();
        }

        if (leftTriggerDown == true || rightTriggerDown == true)
        {
            actor.NetworkUpdateScore(1);
        }

        if (m_gameStart == true)
            {
                StartCoroutine("LoseTime");
                //m_gameStart = true;
            }  
        

        countdownText.text = ("Time Left = " + timeLeft + " sec");

       /* if (timeLeft <= 0)
        {
            StopCoroutine("LoseTime");
            countdownText.text = "GAME OVER!";
            countdownText.color = Color.red;
            Time.timeScale = 0;
        }*/
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    public bool getPinchLeft()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(hand_left.handType);
    }

    public bool getPinchRight()
    {
        return SteamVR_Input._default.inActions.GrabPinch.GetState(hand_right.handType);
    }

    public void StartGame()
    {
        m_gameStart = true;
    }
}
