using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnBoxes : NetworkBehaviour {

    public GameObject boxPrefab;
    public GameObject hierarchy;
    private Vector3[] boxPositions = { new Vector3 { x = 0.3f, y = 1.0f, z = 0.0f },
                                       new Vector3 { x = 0.0f, y = 1.0f, z = 0.0f },
                                       new Vector3 { x = -0.3f, y = 1.0f, z = 0.0f} };


    public override void OnStartServer()
    {
        Spawn();
    }

    private void Spawn()
    {

        for (int i = 0; i < 3; i++)
        {
            // Create the Box from the Box Prefab
            var box = (GameObject)Instantiate(
                boxPrefab,
                boxPositions[i],
                Quaternion.identity);

            //set parent to avoid creating boxes on the root level
            box.transform.parent = gameObject.transform;

            // Spawn the box on the Clients
            NetworkServer.Spawn(box);
        }

    }
}
