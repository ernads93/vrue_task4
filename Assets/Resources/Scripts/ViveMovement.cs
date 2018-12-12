using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveMovement : MonoBehaviour {

    Hand hand_left;
    Hand hand_right;
    bool leftTriggerDown = false;
    bool rightTriggerDown = false;
    bool leftTrackpadDown = false;
    bool rightTrackpadDown = false;

    [SerializeField]
    private Transform rig;

    private Vector3 move_hor = new Vector3(0.0f, 0.0f, 0.002f);
    private Vector3 move_ver = new Vector3(0.002f, 0.0f, 0.0f);

    // Use this for initialization
    void Start () {
        hand_left = gameObject.transform.Find("Controller (left)").GetComponent<Hand>();
        hand_right = gameObject.transform.Find("Controller (right)").GetComponent<Hand>();
    }
	
	// Update is called once per frame
	void Update () {

        leftTriggerDown = getGripLeft();
        rightTriggerDown = getGripRight();
        leftTrackpadDown = getTrackpadLeft();
        rightTrackpadDown = getTrackpadRight();

        if (rightTriggerDown)
        {
            if (rig.position.z > -17.0f)
            { 
                if (rig != null)
                {

                    rig.position += (transform.forward / 10 + move_hor);

                }
            }
        }

        if (leftTriggerDown)
        {
            if (rig.position.z < 9.5f)
            {
                if (rig != null)
                {
                rig.position -= (transform.forward / 10 + move_hor);  
                }
            }
        }

        if (leftTrackpadDown)
        {
            if (rig.position.x > -8.5f)
            {
                if (rig != null)
                {
                    rig.position += (transform.right / 10 + move_ver);

                }
            }
        }

        if (rightTrackpadDown)
        {
            if (rig.position.x < 17.0f)
            {
                if (rig != null)
                {
                    rig.position -= (transform.right / 10 + move_ver);


                }
            }
        }

    }

    public bool getGripLeft()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand_left.handType);
    }

    public bool getGripRight()
    {
        return SteamVR_Input._default.inActions.GrabGrip.GetState(hand_right.handType);
    }

    public bool getTrackpadLeft()
    {
        return SteamVR_Input._default.inActions.Teleport.GetState(hand_left.handType);
    }

    public bool getTrackpadRight()
    {
        return SteamVR_Input._default.inActions.Teleport.GetState(hand_right.handType);
    }
}
