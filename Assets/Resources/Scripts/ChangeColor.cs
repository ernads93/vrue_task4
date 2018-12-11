using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ChangeColor : MonoBehaviour {

    Material m;
    Color defaultColor;
    Actor actor;
    string lower_name;

	// Use this for initialization
	void Start () {

        m = GetComponentInChildren<SkinnedMeshRenderer>().material;
        defaultColor = m.color;
        actor = gameObject.GetComponentInParent<Actor>();
        lower_name = name.ToLower();
    }
	
	// Update is called once per frame
	void Update () {
        if(actor== null)
            actor = gameObject.GetComponentInParent<Actor>();
    }

    void OnTriggerEnter(Collider other)
    {         
        if(m)
            m.color = Color.blue;
        else
        {
            m = GetComponentInChildren<SkinnedMeshRenderer>().material;
            defaultColor = m.color;
            m.color = Color.blue;
        }

        if (actor.isLocalPlayer)
        {
            if (lower_name.Contains("left"))
            {
                //print("trigger left");
                actor.TouchLeft(true);                
            }
            else if (lower_name.Contains("right"))
            {                
                actor.TouchRight(true);              
            }

          //  if (actor.lastCollider != null)
              actor.AssignLastCollider(other.GetComponent<NetworkIdentity>());
        }
    }

    void OnTriggerExit()
    {
        m.color = defaultColor;

        if (actor.isLocalPlayer)
        {
            if (lower_name.Contains("left"))
            {
                actor.TouchLeft(false);
            }
            else if (lower_name.Contains("right"))
            {
                actor.TouchRight(false);
            }

           // actor.lastCollider = null;
        }
    }

    public void ChangeToColor(Color color)
    {
       /* if (m)
            m.color = color;
        else
        {
            m = GetComponentInChildren<SkinnedMeshRenderer>().material;
            defaultColor = m.color;
            m.color = color;
        }*/
    }
}
