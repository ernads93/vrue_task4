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
    Actor actor;
    private GameObject player;

    [SerializeField]
    private Transform rig;

    private Vector3 move_hor = new Vector3(0.0f, 0.0f, 0.002f);
    private Vector3 move_ver = new Vector3(0.002f, 0.0f, 0.0f);

    private Vector3 position = Vector3.zero;

    // Use this for initialization
    void Start () {
        hand_left = gameObject.transform.Find("Controller (left)").GetComponent<Hand>();
        hand_right = gameObject.transform.Find("Controller (right)").GetComponent<Hand>();
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (actor == null)
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");

            actor = player.GetComponentInChildren<Actor>();
        }

        if (actor != null)
           actor.NetworkUpateVivePos(rig.position);

        leftTriggerDown = getGripLeft();
        rightTriggerDown = getGripRight();
        leftTrackpadDown = getTrackpadLeft();
        rightTrackpadDown = getTrackpadRight();

        Vector3 moveMe = Vector3.zero;
        if (rightTriggerDown)
        {
            if (rig.position.z > -13.0f)
            { 
                if (rig != null)
                {

                    moveMe = (transform.forward / 10 + move_hor);

                }
            }
        }

        if (leftTriggerDown)
        {
            if (rig.position.z < 12.5f)
            {
                if (rig != null)
                {
                    moveMe = -(transform.forward / 10 + move_hor);  
                }
            }
        }

        if (leftTrackpadDown)
        {
            if (rig.position.x > -0.5f)
            {
                if (rig != null)
                {
                    //rig.position += (transform.right / 10 + move_ver);
                    moveMe = (transform.right / 10 + move_ver);

                }
            }
        }

        if (rightTrackpadDown)
        {
            if (rig.position.x < 24.0f)
            {
                if (rig != null)
                {
                    // rig.position -= (transform.right / 10 + move_ver);
                    moveMe = -(transform.right / 10 + move_ver);


                }
            }
        }

        //check if in proximity of leap, otherwise hands have to touch and then we can move
        // TODO
        rig.position += moveMe;

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
