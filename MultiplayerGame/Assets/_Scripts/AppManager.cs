using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MANA3DGames
{
    public class AppManager : MonoBehaviour
    {
        public NetworkManager networkManger;

        private void Update()
        {
            if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
            {
                networkManger.StartHost();
            }
                

            if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                networkManger.StartClient();
            }
        }
    }
}
