using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private SoundManager soundManager;
    private void Awake()
    {
        // 마스터 클라이언트가 PhotonNetwork.LoadLevel()을 호출할 수 있도록 하고,
        // 같은 룸에 있는 모든 클라이언트가 레벨을 동기화하게 함
        PhotonNetwork.AutomaticallySyncScene = true;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // 클라이언트가 마스터에 연결되면 호출됨
    public override void OnConnectedToMaster()
    {
        Debug.Log("클라이언트가 마스터에 연결됨");
        // 마스터에 연결되면 방에 랜덤으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    // 클라이언트가 어떤 방식으로든 연결이 끊어지면 호출됨
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("서버와의 연결이 끊어짐. 사유 : {0}", cause);
    }

    // 랜덤 방 입장에 실패할 경우 호출됨
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 방 입장에 실패함. 새로운 방 생성");
        // 랜덤 방 입장에 실패하면 서버 연결이 끊기지 않았다면, 방이 가득 찼거나 방이 없거나이므로 방을 새로 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    // 방에 입장하면 호출됨 
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            GameManager.instance.isHost = false;
        else
            GameManager.instance.isHost = true;
        Debug.Log("Join");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //StartGame();
    }

    bool gameStarted = false;
    public void StartGame()
    {
        if (gameStarted)
            return;
        else
            gameStarted = true;
        PhotonNetwork.LoadLevel("GameScene");
        soundManager.PlayBgm("GameScene");
        Debug.Log("gamescene");
    }


    // Start is called before the first frame update
    void Start()
    {
        Connect();
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
            // 랜덤 룸에 접속.
            // 접속에 실패하면 OnJoinRandomFailed()이 실행되어 실패 알림.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // 서버 연결에 실패하면 서버에 연결 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}