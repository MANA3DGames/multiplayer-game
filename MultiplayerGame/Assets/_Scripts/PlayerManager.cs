using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    public NetworkManager networkManger;

    public override void OnStartClient()
    {
        base.OnStartClient();
        networkManger = GameObject.FindObjectOfType<CustomNetworkManager>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //gameObject.name = "Local";
    }

    private void Update()
    {
        if ( isLocalPlayer )
        {
            if ( Input.GetKeyDown( KeyCode.G ) )
            {
                CmdCreatePlayer( "Green" );
            }

            if ( Input.GetKeyDown( KeyCode.M ) )
            {
                CmdCreatePlayer( "Magneta" );
            }
        }
    }

    [Command]
    void CmdCreatePlayer( string name )
    {
        var body = transform.Find( "Body" );
        GameObject rbody = Instantiate( Resources.Load( name ) as GameObject ) as GameObject;
        rbody.GetComponent<PlayerBody>().owner = gameObject;
        NetworkServer.Spawn( rbody );
    }
}
