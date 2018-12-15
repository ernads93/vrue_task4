using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeapSpawnObj : NetworkBehaviour {

    public GameObject objectPrefab;
    private Character character;
    private GameObject hierarchyObjects;
    public bool createObj;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (character == null)
            character = gameObject.GetComponentInChildren<Actor>().character;

        Debug.Log("createObj: " + createObj);

        if (isLocalPlayer)
        {
            Debug.Log("localPlayer");
            if (createObj || Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("inside create");
                CmdCreateObject();
                createObj = false;
            }
        }
    }

    public void CallObjectCreate()
    {
        createObj = true;
        Debug.Log("createObj callobjcreate: " + createObj);
         
    }

    [Command]
    void CmdCreateObject()
    {

        Debug.Log("spawnObject on server");
        Vector3 spawnPos = (character.left.position + character.right.position) / 2.0f;
        if (hierarchyObjects == null)
            hierarchyObjects = GameObject.FindGameObjectWithTag("Respawn");

        // Create the object from the obj Prefab
        var obj = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        //set parent to avoid creating objects on the root level
         obj.transform.parent = hierarchyObjects.transform;

        // Spawn the object on the Clients
        NetworkServer.Spawn(obj);
    }
}
