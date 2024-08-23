using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject WinAnnounce;
    public GameObject DefeatedAnnounce;
    public GameObject SurrenderText;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        gameObject.SetActive(false);
    }
    public void OnWin()
    {
        ShowWin(false);
    }
    public void OnDefeated()
    {
        ShowDefeated();
    }
    public void OnSurrender()
    {
        ShowDefeated();
        photonView.RPC("ShowWin", RpcTarget.Others, true);
    }
    [PunRPC]
    public void ShowWin(bool isSurrender)
    {
        gameObject.SetActive(true);
        WinAnnounce.SetActive(true);
        if (isSurrender)
            SurrenderText.SetActive(true);
    }
    public void ShowDefeated()
    {
        gameObject.SetActive(true);
        DefeatedAnnounce.SetActive(true);
    }
    public void OnExit()
    {
        Destroy(GameBoard.instance.gameObject);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LeaveRoom();
    }
}
