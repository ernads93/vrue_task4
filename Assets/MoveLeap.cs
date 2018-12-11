using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeap : MonoBehaviour {

    private GameObject Leapcamera;
    private GameObject player;
    private bool moveUp;
    private bool collisionWithWall;
    private bool rotateRight;
    private bool rotateLeft;
    public Leap.Unity.HandModelBase left;
    public Leap.Unity.HandModelBase right;

    private Leap.Hand handLeft;
    private Leap.Hand handRight;

    // Use this for initialization
    void Start () {        
        Leapcamera = GameObject.FindGameObjectWithTag("MainCamera");
       
    }
	
	// Update is called once per frame
	void Update () {
        if(Leapcamera!=null)
            Leapcamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        if (rotateRight)
        {
            if(handRight == null)
                handRight = right.GetLeapHand();

            //Vector3 dir = new Vector3(handRight.PalmNormal.x, handRight.PalmNormal.y, handRight.PalmNormal.z);
            Vector3 dir = new Vector3(0.0f, 0.5f, 0.0f);
            Leapcamera.transform.Rotate(dir);
        }
        else if (rotateLeft) {
            if (handLeft== null)
                handLeft = left.GetLeapHand();
            
           Vector3 dir = new Vector3(handLeft.PalmNormal.x, handLeft.PalmNormal.y, handLeft.PalmNormal.z);
            //Vector3 dir = new Vector3(0.0f, -0.5f, 0.0f);
            Leapcamera.transform.Rotate(dir);            
            
        }
        else if (moveUp)
        {
            if (handRight== null)
                handRight = right.GetLeapHand();

            Vector3 dir = new Vector3(handRight.PalmNormal.x, handRight.PalmNormal.y, handRight.PalmNormal.z);
            Leapcamera.transform.position += dir; new Vector3(0.0f, 0.0f, handRight.PalmNormal.z/100.0f);
            //Leapcamera.transform.position += dir/100.0f;
        }
    }

    public void StartMoveUp() {
        print("MoveUp");
        if(!collisionWithWall)
            moveUp = true;
    }

    public void StopMoveUp() {
        print("Stop MovingUp");
        
        moveUp = false;
    }

    public void EnableRotateLeft()
    {
        print("start rotate left");
        rotateLeft = true;
    }

    public void DisableRotateLeft()
    {
        print("Stop rotate left");

        rotateLeft = false;
    }
    public void EnableRotateRight()
    {
        print("start rotate left");
        rotateRight = true;
    }

    public void DisableRotateRight()
    {
        print("Stop rotate right");

        rotateRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("ontrigger enter");
        if (other.gameObject.layer == 8)
            collisionWithWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
            collisionWithWall = false;
    }

}
