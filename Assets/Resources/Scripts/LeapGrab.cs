using UnityEngine;
using System.Collections;

// This script defines conditions that are necessary for the Leap player to grab a shared object
// TODO: values of these four boolean variables can be changed either directly here or through other components
// AuthorityManager of a shared object should be notifyed from this script

public class LeapGrab : MonoBehaviour
{

    AuthorityManager am;

    // conditions for the object control here
    bool leftHandTouching = false;
    bool rightHandTouching = false;
    bool leftPinch = false;
    bool rightPinch = false;
    Actor actor;
    GameObject player;
    // Use this for initialization
    void Start()
    {     
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (actor == null)
        {      
            actor = gameObject.GetComponentInChildren<Actor>();
            if(actor != null)
                actor.leapStatus = true;
        }

        if (leftHandTouching || rightHandTouching)// && leftPinch && rightPinch)
        {
            // notify AuthorityManager that grab conditions are fulfilled
            //print("LEAP IS GRABABLE");

            if (actor != null && actor.lastCollider != null)
            {
               // print("lastcollider is: " + actor.lastCollider);
                am = actor.lastCollider.GetComponent<AuthorityManager>();
                am.grabbedByPlayer = true;
            }           
        }
        else
        {
            // grab conditions are not fulfilled
            if (am != null)
                am.grabbedByPlayer = false;
        }
    }

    public void SetPinchTrue()
    {
        leftPinch = true;
        rightPinch = true;
        var script = player.GetComponentInChildren<LeapSpawnObj>();
        if(script != null)
        {
            
                var character = player.GetComponentInChildren<Actor>().character;
            
            Vector3 spawnPos = (character.left.position + character.right.position) / 2.0f;
            Debug.Log("inside create");

            script.CallObjectCreate(spawnPos, 1);
        }

        //print("pinch true");
    }

    public void SetPinchFalse()
    {
        leftPinch = false;
        rightPinch = false;
        // print("pinch left false");
    }

    public void SetRightHandTouchState(bool state)
    {
        rightHandTouching = state;
       /* if (state)
            print("right hand touch");*/
    }

    public void SetLeftHandTouchState(bool state)
    {
        leftHandTouching = state;
       /* if (state)
            print("left hand touch");*/
    }

}
