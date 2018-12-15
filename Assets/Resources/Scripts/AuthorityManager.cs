using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// TODO: this script should manage authority for a shared object
public class AuthorityManager : NetworkBehaviour
{


    private NetworkIdentity netID; // NetworkIdentity component attached to this game object

    // these variables should be set up on a client
    //**************************************************************************************************
    Actor localActor; // Actor that is steering this player 

    private bool grabbed = false; // if this is true client authority for the object should be requested
    public bool grabbedByPlayer // private "grabbed" field can be accessed from other scripts through grabbedByPlayer
    {
        get { return grabbed; }
        set { grabbed = value; }
    }

    OnGrabbedBehaviour onb; // component defining the behaviour of this GO when it is grabbed by a player
                            // this component can implement different functionality for different GO´s

    //***************************************************************************************************

    // these variables should be set up on the server

    // TODO: implement a mechanism for storing consequent authority requests from different clients
    // e.g. manage a situation where a client requests authority over an object that is currently being manipulated by another client
    List<NetworkConnection> requestAuth;
    List<NetworkConnection> releaseAuth;

    //*****************************************************************************************************

    // TODO: avoid sending two or more consecutive RemoveClientAuthority or AssignClientAUthority commands for the same client and shared object
    // a mechanism preventing such situations can be implemented either on the client or on the server
    bool pendingAccess = false;
    bool pendingRelease = false;
    public Text textField;

    // Use this for initialization
    void Start()
    {
        netID = GetComponent<NetworkIdentity>();
        onb = GetComponent<OnGrabbedBehaviour>();
        requestAuth = new List<NetworkConnection>();
        releaseAuth = new List<NetworkConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)//client
        {            
            if (hasAuthority)  //we should now be able to move the object
            {
                pendingAccess = false;

                 print("we have authority");
                if (grabbedByPlayer)
                {
                    // print(name + " id: " + netId);
                    if (localActor.lastCollider != null) //both hands are touching the same object
                    {
                        //print("both collider set");

                        onb.setColor(Color.red);
                        onb.vive = localActor.viveStatus;
                        onb.leap = localActor.leapStatus;
                        onb.setActor(localActor);
                        onb.OnGrabbed();
                    }
                }
                else
                {
                    if (!pendingRelease)
                    {
                        onb.OnReleased();
                        onb.setColor(Color.green);
                        print("we return auhtority: ");
                        if(localActor!=null)
                            localActor.ReturnObjectAuthority(netID);
                        pendingRelease = true;
                    }                           
                }
            }
            else if(pendingRelease)
            {
                pendingRelease = false;
            }
            else if (grabbedByPlayer && !pendingAccess)
            {
                print("we want control");
                localActor.RequestObjectAuthority(netID);
                pendingAccess = true;
            }          
        }

    }

    // assign localActor here
    public void AssignActor(Actor actor)
    {
        localActor = actor;
    }

    // should only be called on server (by an Actor)
    // assign the authority over this game object to a client with NetworkConnection conn
    public void AssignClientAuthority(NetworkConnection conn)
    {
        if (netID.clientAuthorityOwner == null) //no client has authority
        {
           netID.AssignClientAuthority(conn);
        }
        else
        {
            requestAuth.Add(conn);
        }
    }

    // should only be called on server (by an Actor)
    // remove the authority over this game object from a client with NetworkConnection conn
    public void RemoveClientAuthority(NetworkConnection conn)
    {
        if(netID.clientAuthorityOwner == conn) // client controlling object wants to release authority
        {
            netID.RemoveClientAuthority(conn);

            if (requestAuth.Count != 0) // we have pending ones
            {               
                NetworkConnection next = requestAuth[0];
                requestAuth.RemoveAt(0);
                AssignClientAuthority(next); //can process sequentially                         
            }
        }
        else
        {
            if (requestAuth.Contains(conn))
            {
                requestAuth.Remove(conn);
            }
            else
            {
                releaseAuth.Add(conn);
            }
           
        }
        
    }

}
