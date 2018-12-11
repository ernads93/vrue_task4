using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NetworkController : NetworkManager
{
  
    public bool host;
    public bool server;

    public GameObject boxPrefab;
    public GameObject hierarchy;
    private Vector3[] boxPositions = { new Vector3 { x = 0.3f, y = 1.0f, z = 0.0f }, 
                                       new Vector3 { x = 0.0f, y = 1.0f, z = 0.0f }, 
                                       new Vector3 { x = -0.3f, y = 1.0f, z = 0.0f} };
    
    public static NetworkController FindInstance()
    {
        return FindObjectOfType<NetworkController>();
    }

    public class NetMsgType
    {
        public const short EmptyMessage = MsgType.Highest + 1; // Empty network message

        public class EmptyMessageMsg : MessageBase { }
    }

    /// <summary>
    /// Connect to server or create host server.
    /// </summary>
    private void Start()
    {

        if(server)
        {
            StartServer(); 
        }
        else if(host)
        {
            StartHost();
        }
        else
        {
            StartClient();
        }

    }
             
    // overriden functions implement only base functionality; however, additional functionality can be implemented here
    public override void OnStartServer()
    {
        base.OnStartServer();  
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        //SpawnBoxes();
    }
    private void SpawnBoxes() {

        for (int i = 0; i < 3; i++) {
            // Create the Box from the Box Prefab
            var box = (GameObject)Instantiate(
                boxPrefab,
                boxPositions[i],
                Quaternion.identity);

            //set parent to avoid creating boxes on the root level
            box.transform.parent = hierarchy.transform;

            // Spawn the box on the Clients
            NetworkServer.Spawn(box);
        }

    }
}

