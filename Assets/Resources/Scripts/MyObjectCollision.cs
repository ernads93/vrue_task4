using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObjectCollision : MonoBehaviour {

    public GameObject cameraLeap;
    private MoveLeap script;

	// Use this for initialization
	void Start () {
        script = cameraLeap.GetComponentInChildren<MoveLeap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("ontrigger enter");
       // if (other.gameObject.layer == 8)
            script.collisionWithWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
            script.collisionWithWall = false;
    }

}
