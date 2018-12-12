using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class StartGame : MonoBehaviour {

    private bool leftTriggerDown = false;
    private bool rightTriggerDown = false;

    public Hand hand_left;
    public Hand hand_right;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        leftTriggerDown = getPinchLeft();
        rightTriggerDown = getPinchRight();

        if (leftTriggerDown == true || rightTriggerDown == true)
        {
            Destroy(gameObject);
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

}
