using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeap : MonoBehaviour {

    private GameObject Leapcamera;
    private GameObject player;
    private bool moveUp;
    public bool collisionWithWall { get; set; }
    public bool rotateRight { get; set; }
    public bool rotateLeft { get; set; }
    public Leap.Unity.HandModelBase left;
    public Leap.Unity.HandModelBase right;

    private Leap.Hand handLeft;
    private Leap.Hand handRight;


    Actor actor;
    // Use this for initialization
    void Start () {        
        Leapcamera = GameObject.FindGameObjectWithTag("MainCamera");
       
    }
	
	// Update is called once per frame
	void Update () {

        if(Leapcamera==null)
            Leapcamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        if(actor == null)
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");

            actor = player.GetComponentInChildren<Actor>();
        }       

        if (rotateRight)
        {
            if(handRight == null)
                handRight = right.GetLeapHand();

            Leapcamera.transform.RotateAround(this.transform.position, Vector3.up, 30 * Time.deltaTime);
        }
        else if (rotateLeft) {
            if (handLeft== null)
                handLeft = left.GetLeapHand();

            Leapcamera.transform.RotateAround(this.transform.position, Vector3.up, -30 * Time.deltaTime);
        }
        else if (moveUp)
        {
            if (handRight== null)
                handRight = right.GetLeapHand();

            Vector3 dir = new Vector3(handRight.PalmNormal.x, 0.0f, handRight.PalmNormal.z);
            Vector3 cameraPos = Leapcamera.transform.position;
            Vector3 newPos = cameraPos+(dir/60.0f);
            Vector3 checkPos = cameraPos+(dir/10.0f);
            
            if(actor!= null)
                actor.NetworkUpateLeapPos(checkPos);            
           
           // Debug.Log("checkPos: " + checkPos + " newPos: " + newPos);
            Debug.Log("distance: " + actor.getDistanceToOtherPlayer());

            // print(Vector3.Distance(newPos, Vector3.zero));
           if (actor.getDistanceToOtherPlayer() < 2.5)
            {
                Leapcamera.transform.position = newPos;
            } 
        }
    }

    public void StartMoveUp() {
       //print("MoveUp");
        if(!collisionWithWall)
            moveUp = true;
    }

    public void StopMoveUp() {
       // print("Stop MovingUp");       
        moveUp = false;
    }

    public void EnableRotateLeft()
    {
       // print("start rotate left");
        rotateLeft = true;
    }

    public void DisableRotateLeft()
    {
        //print("Stop rotate left");
        rotateLeft = false;
    }
    public void EnableRotateRight()
    {
        //print("start rotate left");
        rotateRight = true;
    }

    public void DisableRotateRight()
    {
        //print("Stop rotate right");
        rotateRight = false;
    }

}
