using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

// TODO: define the behaviour of a shared object when it is manipulated by a client

public class OnGrabbedBehaviour : MonoBehaviour
{


    bool grabbed;

    public bool vive { get; set; }
    public bool leap { get; set; } 
    private Actor actor;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {      
        // GO´s behaviour when it is in a grabbed state (owned by a client) should be defined here
        if (grabbed)
        {
          
            {

                //print("on grab move: ");
                Character character = actor.GetComponentInChildren<Character>();

                // midpoint between left/right character hands              
                transform.position = (character.left.position + character.right.position) / 2;

                GetComponent<Rigidbody>().useGravity = false;
            }                
            //gameObject.GetComponent<Renderer>().material.color = Color.blue;         
           
        }
        if (!grabbed)
        {
            GetComponent<Rigidbody>().useGravity = true;
            // gameObject.transform.SetParent(null);
            //  gameObject.GetComponent<Renderer>().material.color = Color.green;
        }


    }

    public void setColor(Color c)
    {
        gameObject.GetComponent<Renderer>().material.color = c;
    }

    public void setActor(Actor ac)
    {
        actor = ac;
    }
    // called first time when the GO gets grabbed by a player
    public void OnGrabbed()
    {
        grabbed = true;
    }

    // called when the GO gets released by a player
    public void OnReleased()
    {
        grabbed = false;
    }
}
