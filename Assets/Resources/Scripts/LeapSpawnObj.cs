using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeapSpawnObj : NetworkBehaviour {

    public GameObject objectPrefab1;
    public GameObject objectPrefab2;
    private Character character;
    private GameObject hierarchyObjects;
    public bool createObj;

    // Use this for initialization
    void Start () {
        createObj = false;

    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void CallObjectCreate(Vector3 spawnPos, int what)
    { 

            CmdCreateObject(spawnPos, what);
        
    }

    [Command]
    void CmdCreateObject(Vector3 spawnPos, int what)
    {

        Debug.Log("spawnObject on server");
        GameObject objectPrefab;
        switch(what)
        {
            /*case 1: objectPrefab = objectPrefab1;
                break;*/
            case 2:
                objectPrefab = objectPrefab2;
                break;
            default:
                objectPrefab = objectPrefab1;
                break;
        }

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
