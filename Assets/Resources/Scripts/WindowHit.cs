﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
            Destroy(other.gameObject);
    }
}
