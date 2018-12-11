using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// TODO: this script CAN be used to detect the events of a left networked hand touching a shared object
// fill in the implementation and communicate touching events to either LeapGrab and ViveGrab by setting the rightHandTouching variable
// ALTERNATIVELY, implement the verification of the grabbing conditions in a way  your prefer
// TO REMEMBER: only the localPlayer (networked hands belonging to the localPlayer) should be able to "touch" shared objects

public class TouchLeft : MonoBehaviour
{

    public bool vive;
    public bool leap;

    LeapGrab leapGrab;

    public void Start()
    {
       leapGrab = GetComponentInParent<LeapGrab>();
    }
    public void SetLeftHandTouch(bool state)
    {
        if (leapGrab == null)
            leapGrab = GetComponentInParent<LeapGrab>();

        if (leap)
        {                        
            leapGrab.SetLeftHandTouchState(state);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Left Touch");
        if(vive)
        this.GetComponentInParent<ViveGrab>().setLeftHandTouching();
    }

    private void OnTriggerExit(Collider other)
    {
      // Debug.Log("Left not touch");
        if(vive)
        this.GetComponentInParent<ViveGrab>().removeLefttHandTouching();
    }
}