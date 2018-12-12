using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowHit : MonoBehaviour {

    public Text m_scoreText;
    private int m_points = 0;
    private bool triggered = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_points = Convert.ToInt32(m_scoreText.text.Substring(m_scoreText.text.Length - 2));
        if (triggered)
        {
            if (gameObject.tag == "window1")
            {
                m_points += 2;
            }
            if (gameObject.tag == "window2")
            {
                m_points += 4;
            }
            if (gameObject.tag == "window3")
            {
                m_points += 6;
            }
            if (gameObject.tag == "window4")
            {
                m_points += 8;
            }
            if (gameObject.tag == "window5")
            {
                m_points += 10;
            }
            if (gameObject.tag == "window6")
            {
                m_points += 12;
            }
            if (gameObject.tag == "window7")
            {
                m_points += 14;
            }
            triggered = false;
        }

        m_scoreText.text = ("Points: " + m_points);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            Destroy(other.gameObject);
            triggered = true;
        }
    }
}
