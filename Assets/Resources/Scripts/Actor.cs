using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Actor : NetworkBehaviour
{
    [SyncVar(hook = "UpdatePlayerPositions")]
    float distanceToOtherPlayer = 0.0f;

    [SyncVar]
    int score = -1;


    public float getDistanceToOtherPlayer() { return distanceToOtherPlayer; }
    Vector3 LeapPos;
    Vector3 VivePos = new Vector3(4.16f, 0.6f,0.92f);

    public Character character;
    public new Transform transform;
    GameObject playerTag; // this should either be the Leap or VIVE
    TouchRight tr;
    TouchLeft tl;
    private bool vive;
    private bool leap;

    Text timerText;
    Timer timer;
    Text pointsText;

    // public GameObject capsulePrefab;
    // private GameObject hierarchyObjects;

    public bool leapStatus
    {
        get { return leap; }
        set { leap = value; }
    }

    public bool viveStatus
    {
        get { return vive; }
        set { vive = value; }
    }
    public NetworkIdentity lastCollider;

    [SyncVar]
    private string prefabName = "";

    //this part is for object sharing
    //*******************************
    List<NetworkIdentity> sharedObjects; // shared objects on the server or localActor

    public bool wantsAuthority { get; set; }

    //*******************************
    //private CreateManipObj createObjects;


    protected virtual void Awake()
    {
        transform = base.transform;
    }

    // Use this for initialization
    void Start()
    {
        //hierarchyObjects = GameObject.FindGameObjectWithTag("Respawn");

        if (isServer || isLocalPlayer)
        {
            if (isLocalPlayer)
            {
                // Inform the local player about his new character
                var localSingleton = LocalPlayerController.Singleton;
                // if (localSingleton != null)
                {
                    localSingleton.SetActor(this);
                    CmdInitialize(prefabName);
                    wantsAuthority = false;
                }
            }

            //this part is for object sharing
            //*******************************
            // actually both want the list of distributed items
            GameObject[] obs = GameObject.FindGameObjectsWithTag("shared");
            sharedObjects = new List<NetworkIdentity>();

            foreach (GameObject o in obs)
            {
                sharedObjects.Add(o.GetComponent<NetworkIdentity>());
                //  NetworkServer.Spawn(o);
            }

            if (isServer)
            {
                // find objects that can be manipulated 
                // TIPP : you can use a specific tag for all GO's that can be manipulated by players                               
            }
            if (isLocalPlayer)
            {
                // find objects that can be manipulated 
                // assign this Actor to the localActor field of the AuthorityManager component of each shared object
                if (obs == null)
                    obs = GameObject.FindGameObjectsWithTag("shared");
                {
                    foreach (GameObject o in obs)
                    {
                        o.GetComponent<AuthorityManager>().AssignActor(this);
                    }
                }

                var timerObj = GameObject.FindGameObjectWithTag("Timer");
                timerText = timerObj.GetComponentInChildren<Text>();
                timer = timerObj.GetComponentInChildren<Timer>();
                pointsText = GameObject.FindGameObjectWithTag("Points").GetComponentInChildren<Text>();

            }
            //*******************************

        }
        else
        {
            // Initialize on startup
            Initialize(prefabName);
        }
        playerTag = GameObject.FindGameObjectWithTag("Player");

        // include inactive children, as the capsule hands are only set active 
        // if Leap rexognizes your hands
        tr = playerTag.GetComponentInChildren<TouchRight>(true);
        tl = playerTag.GetComponentInChildren<TouchLeft>(true);
        //createObjects = gameObject.GetComponentInChildren<CreateManipObj>();
    }

    public void Update()
    {

        if (!isLocalPlayer)
            return;

        checkScore();
        //UpdateScore();
    }



    /// <summary>
    /// Updates the actor position and rotation.
    /// This function should be called only by the <see cref="LocalPlayerController"/>.
    /// </summary>
    public void UpdateActorLeft(Vector3 leftPos, Quaternion leftRot) //runs only on LocalPlayer!
    {
        if (character != null)
        {
            character.UpdateCharacterLeft(leftPos, leftRot);
        }

    }

    /// <summary>
    /// Updates the actor position and rotation.
    /// This function should be called only by the <see cref="PlayerController"/>.
    /// </summary>
    public void UpdateActorRight(Vector3 rightPos, Quaternion rightRot) //runs only on LocalPlayer!
    {
        if (character != null)
        {
            character.UpdateCharacterRight(rightPos, rightRot);
        }
    }

    public void SetRightCharacterActive(bool active)
    {
        character.SetRightActive(active);
    }

    public void SetLeftCharacterActive(bool active)
    {
        character.SetLeftActive(active);
    }

    /// <summary>
    /// Initialize the player locally.
    /// </summary>
    /// <param name="prefab">Prefab character name.</param>
    public void Initialize(string prefab)
    {
        prefabName = prefab;
        name = name.Replace("(Clone)", "");

    }

    /// <summary>
    /// Spawns the character of actor on all clients.
    /// This runs on server only.
    /// </summary>
    /// <param name="prefab">Prefab name of the character.</param>
    private void SpawnCharacter(string prefab)
    {
        // Spawn character
        GameObject modelPrefab = Resources.Load("Prefabs/" + prefab) as GameObject;
        GameObject model = (GameObject)Instantiate(modelPrefab, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(model);

        // Attach character to player
        AttachCharacter(model.GetComponent<Character>());
    }

    /// <summary>
    /// Initializes the character on server to inform all clients. 
    /// This command calls the Initialize() method and spawns the character.
    /// </summary>
    [Command]
    public void CmdInitialize(string prefab)
    {
        if (prefab.Length > 0)
        {
            CreateCharacter(prefab);
        }
    }

    /// <summary>
    /// Creates the character and initializes on server.
    /// </summary>
    /// <param name="prefab">The character prefab name.</param>
    [Server]
    public void CreateCharacter(string prefab)
    {
        SpawnCharacter(prefab);
        Initialize(prefab);
    }

    /// <summary>
    /// Main routine to attach the character to this actor
    /// This runs only on Server.
    /// </summary>
    /// <param name="newCharacter">New character to attach.</param>
    [Server]
    public void AttachCharacter(Character newCharacter)
    {
        newCharacter.AttachToActor(netId);
    }


    //this part is for object sharing
    // fill in the implementation
    //*******************************

    // should only be run on localPlayer (by the AuthorityManager of a shared object)
    // ask the server for the authority over an object with NetworkIdentity netID
    public void RequestObjectAuthority(NetworkIdentity netID)
    {
        CmdAssignObjectAuthorityToClient(netID);
    }

    // should only be run on localPlayer (by the AuthorityManager of a shared object)
    // ask the server to remove the authority over an object with NetworkIdentity netID
    public void ReturnObjectAuthority(NetworkIdentity netID)
    {
        CmdRemoveObjectAuthorityFromClient(netID);
    }

    // run on the server
    // netID is NetworkIdentity of a shared object the authority if which should be passed to the client
    [Command]
    void CmdAssignObjectAuthorityToClient(NetworkIdentity netID)
    {
        var am = netID.GetComponent<AuthorityManager>();
        am.AssignClientAuthority(base.connectionToClient);
    }

    // run on the server
    // netID is NetworkIdentity of a shared object the authority if which should be removed from the client
    [Command]
    void CmdRemoveObjectAuthorityFromClient(NetworkIdentity netID)
    {
        var am = netID.GetComponent<AuthorityManager>();
        am.RemoveClientAuthority(base.connectionToClient);
    }
    //*******************************

    public void TouchLeft(bool state)
    {
        tl.SetLeftHandTouch(state);
        //   print("is touch left? - " + (state ? " yes" : " no"));
    }

    public void TouchRight(bool state)
    {
        tr.SetRightHandTouch(state);
        // print("is touch right? - " + (state ? " yes" : " no"));
    }

    public void AssignLastCollider(NetworkIdentity other)
    {
        // if (lastCollider == null)
        {
            lastCollider = other;
            var ogb = other.gameObject.GetComponent<OnGrabbedBehaviour>();
            if (ogb != null)
                ogb.setActor(this);
        }
        /*       else
                {   // iff ids are different, hand are touching different boxes
                    //print((lastCollider.netId==other.netId)  + "lastcollier id: " + lastCollider.netId + " newly id: " + other.netId);
                    lastCollider = (lastCollider.netId == other.netId) ? other : null;
                }*/
    }

    void UpdatePlayerPositions(float newDist)
    {
        distanceToOtherPlayer = newDist;
    }

    public void NetworkUpateVivePos(Vector3 pos)
    {
        //VivePos = pos;
        // updateDistance();
        CmdVivePosUpdate(pos);
    }

    [Command]
    void CmdVivePosUpdate(Vector3 pos)
    {
        VivePos = pos;
        Debug.Log("VivePOs: " + VivePos);
        distanceToOtherPlayer = Vector3.Distance(VivePos, LeapPos);
    }


    public void NetworkUpateLeapPos(Vector3 pos)
    {
        // LeapPos = pos;
        // updateDistance();        
        CmdLeapPosUpdate(pos);
    }

    [Command]
    void CmdLeapPosUpdate(Vector3 pos)
    {
        LeapPos = pos;
        Debug.Log("LeapPOs: " + LeapPos);
        distanceToOtherPlayer = Vector3.Distance(VivePos, LeapPos);
    }

    public void NetworkUpdateScore(int add)
    {
        if (isLocalPlayer)
            CmdUpdateScore(add);
    }

    [Command]
    void CmdUpdateScore(int add)
    {
        if (score == -1)
        {
            score = 0;
        }
        else
        {
            score += add;
        }
        RpcUpdateScore(score);
        //UpdateScore(score);
    }

    void updateDistance()
    {
        /* if (!isServer)
             return;
             */
        if (VivePos != null && LeapPos != null)
        {

            //VivePos = dummyVive.transform.position;
            distanceToOtherPlayer = Vector3.Distance(VivePos, LeapPos);
            Debug.Log("distance server: " + distanceToOtherPlayer);
        }
    }

    void checkScore()
    {

        if (score == 0)
        {
            timer.StartGame();
        }

        pointsText.text = ("Points: " + score);
    }

    [ClientRpc]
    void RpcUpdateScore(int newScore)
    {
        if (!isServer)
            return;

        if (newScore == 0)
        {
            timer.StartGame();
        }

        score = newScore;
        pointsText.text = ("Points: " + score);
    }

    public int getScore()
    {
        return score;

    }
}