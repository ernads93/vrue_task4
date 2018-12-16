using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;

// This script defines conditions that are necessary for the Vive player to grab a shared object
// TODO: values of these four boolean variables can be changed either directly here or through other components
// AuthorityManager of a shared object should be notifyed from this script

public class ViveGrab : MonoBehaviour {

    AuthorityManager am; // to communicate the fulfillment of grabbing conditions
    Hand hand_left;
    Hand hand_right;
    Actor actor;

    // conditions for the object control here
    bool leftHandTouching = false;
    bool rightHandTouching = false;
    bool leftTriggerDown = false;
    bool rightTriggerDown = false;
    

    // Use this for initialization
    void Start () {
        hand_left = gameObject.transform.Find("Controller (left)").GetComponent<Hand>();
        hand_right = gameObject.transform.Find("Controller (right)").GetComponent<Hand>();
    }
	
	// Update is called once per frame
	void Update () {

        if (actor == null)
        {
            actor = gameObject.GetComponentInChildren<Actor>(); //therefore the script must be added to VIVE-Scene object
            if (actor != null)
                actor.viveStatus = true;
        }

        leftTriggerDown = getPinchLeft();
        rightTriggerDown = getPinchRight();

        if (leftHandTouching && rightHandTouching) // && leftTriggerDown && rightTriggerDown)
        {
            // notify AuthorityManager that grab conditions are fulfilled
            if (actor.lastCollider != null)
            {
              //  print("lastcollider is: " + actor.lastCollider);
                am = actor.lastCollider.GetComponent<AuthorityManager>();
                am.grabbedByPlayer = true;
            }         
           // Debug.Log("SUCCESS");
        }
        else
        {
            // grab conditions are not fulfilled
            if (am != null)
                am.grabbedByPlayer = false;
        }

    }

    public void setRightHandTouching()
    {
        rightHandTouching = true;
        //Debug.Log(rightHandTouching);
    }

    public void removeRightHandTouching()
    {
        rightHandTouching = false;
        //Debug.Log(rightHandTouching);
    }

    public void setLeftHandTouching()
    {
        leftHandTouching = true;
       // Debug.Log(leftHandTouching);
    }

    public void removeLefttHandTouching()
    {
        leftHandTouching = false;
       // Debug.Log(leftHandTouching);
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
