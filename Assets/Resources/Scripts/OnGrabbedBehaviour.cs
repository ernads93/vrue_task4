using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;
using Leap.Unity.Interaction;
// TODO: define the behaviour of a shared object when it is manipulated by a client

public class OnGrabbedBehaviour : MonoBehaviour
{


    bool grabbed;

    public bool vive { get; set; }
    public bool leap { get; set; } 
    private Actor actor;
    InteractionBehaviour leapInteract;
    Interactable viveInteract;

    InteractionManager manager;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (manager == null) {            
            manager = GameObject.FindObjectOfType<InteractionManager>();
        }

        if (viveInteract == null)
            viveInteract= gameObject.GetComponentInChildren<Interactable>();


        // GO´s behaviour when it is in a grabbed state (owned by a client) should be defined here
        if (grabbed)
        {
          
            {

                Debug.Log("is this leap: " + leap);
                Character character = actor.GetComponentInChildren<Character>();

                if (leap)
                {
                    leapInteract = gameObject.GetComponentInChildren<InteractionBehaviour>();
                    if (leapInteract == null)
                    {
                        
                        leapInteract = gameObject.AddComponent<InteractionBehaviour>();
                        leapInteract.manager = manager;
                    }
                }

                if (vive)
                    viveInteract.enabled = true;
                // midpoint between left/right character hands              
                //transform.position = (character.left.position + character.right.position) / 2;

                //GetComponent<Rigidbody>().useGravity = false;
            }                
            //gameObject.GetComponent<Renderer>().material.color = Color.blue;         
           
        }
        if (!grabbed)
        {
            //GetComponent<Rigidbody>().useGravity = true;
            // gameObject.transform.SetParent(null);
            //  gameObject.GetComponent<Renderer>().material.color = Color.green;

            if (leap)
            {
               // leapInteract.enabled = false;
            }

            /*if (vive)
                viveInteract.enabled = false;*/
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
