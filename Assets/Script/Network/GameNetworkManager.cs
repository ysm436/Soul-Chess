using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkManager : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        GameManager.instance.LoadMainScene();
        GameBoard.instance = null;
    }
}
