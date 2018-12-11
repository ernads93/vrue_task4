using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// TODO: this script CAN be used to detect the events of a right networked hand touching a shared object
// fill in the implementation and communicate touching events to either LeapGrab and ViveGrab by setting the rightHandTouching variable
// ALTERNATIVELY, implement the verification of the grabbing conditions in a way  your prefer
// TO REMEMBER: only the localPlayer (networked hands belonging to the localPlayer) should be able to "touch" shared objects

public class TouchRight : MonoBehaviour
{
    // the implementation of a touch condition might be different for Vive and Leap 
    public bool vive;
    public bool leap;

    LeapGrab leapGrab;

    public void Start()
    {
      // leapGrab = GetComponentInParent<LeapGrab>();
    }
    public void SetRightHandTouch(bool state)
    {
        if(leapGrab == null)
            leapGrab = GetComponentInParent<LeapGrab>();

        if (leap)
        {        
            leapGrab.SetRightHandTouchState(state);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Right Touch");
        if(vive)
           this.GetComponentInParent<ViveGrab>().setRightHandTouching();

    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Right not touch");
        if(vive)
            this.GetComponentInParent<ViveGrab>().removeRightHandTouching();
    }

}