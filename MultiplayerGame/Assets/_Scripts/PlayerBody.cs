using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerBody : NetworkBehaviour
{
    public GameObject owner;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if ( !owner )
        {
            Debug.Log( "There is no OWNER!!!!!!!!!!!!!!!!!!" );
            //RpcSetParent();
            return;
        }

        var body = owner.transform.Find( "Body" );
        transform.SetParent( body );
        transform.localPosition = body.localPosition;
    }


    //[ClientRpc]
    //public void RpcSetParent()
    //{
    //    var body = GameObject.Find( "Player(Clone)" ).transform.Find( "Body" );
    //    transform.SetParent( body );
    //    transform.localPosition = body.localPosition;
    //}
}
