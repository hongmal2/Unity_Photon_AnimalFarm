using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPhotonManager : Photon.PunBehaviour
{
    public GameObject ExitButton;

    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MyExitRoom()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }

    public void MyStartGame()
    {
        if(PhotonNetwork.isMasterClient)
        PhotonNetwork.LoadLevel(2);
    }

}
