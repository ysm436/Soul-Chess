using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public LobbySceneUI lobbySceneUI;

    public GameObject loadingView;
    private void Start()
    {
        Connect();
        if (PhotonNetwork.CurrentRoom != null)
        {
            lobbySceneUI.Init();
        }
    }

    // 방에 입장하면 호출됨 
    public override void OnJoinedRoom()
    {
        lobbySceneUI.Init();
        Debug.Log("Join");
    }

    bool gameStarted = false;
    public void StartGame()
    {
        if (gameStarted)
            return;
        else
            gameStarted = true;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("GameScene");
        Debug.Log("gamescene");
    }

    /// <summary>
    /// 연결 프로세스를 시작.
    /// 이미 연결이 되어있다면, 무작위 룸으로
    /// 연결이 되지 않았다면 다시 연결
    /// </summary>
    public void Connect()
    {
        // 연결 되었는지를 체크해서, 룸에 참여할지 재연결을 시도할지 결정
        if (PhotonNetwork.IsConnected)
        {
        }
        else
        {
            // 서버 연결에 실패하면 서버에 연결 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ExitButton()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1) { }
        PhotonNetwork.LeaveRoom();
        loadingView.SetActive(false);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        GameManager.instance.LoadMatchingScene();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            lobbySceneUI.OnOtherPlayerJoined();
        }
    }
}