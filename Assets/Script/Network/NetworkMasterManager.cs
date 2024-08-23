using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class NetworkMasterManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomButtonPrefab;
    public TextMeshProUGUI CurrentRoomText;
    bool roomSetCorrectly = false;
    public Transform roomButtonParentTransform;
    public TMP_InputField roomName;
    public TextMeshProUGUI roomnameAnounceText;
    public GameObject LoadingView;


    public bool roomCreatingPanelActive
    {
        set
        {
            if (value)
                _roomCreatingPanel.SetActive(true);
            else if (isCreatingRoom) return;
            else _roomCreatingPanel.SetActive(false);
        }
    }
    [SerializeField] private GameObject _roomCreatingPanel;

    bool isCreatingRoom = false;

    private Dictionary<string, GameObject> localRoomDict = new();

    private void Awake()
    {
        // 마스터 클라이언트가 PhotonNetwork.LoadLevel()을 호출할 수 있도록 하고,
        // 같은 룸에 있는 모든 클라이언트가 레벨을 동기화하게 함
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InLobby)
        {
            LoadingView.SetActive(false);
        }

    }    // Start is called before the first frame update
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
            /*
            // 랜덤 룸에 접속.
            // 접속에 실패하면 OnJoinRandomFailed()이 실행되어 실패 알림.
            PhotonNetwork.JoinRandomRoom();
            */
        }
        else
        {
            // 서버 연결에 실패하면 서버에 연결 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 클라이언트가 마스터에 연결되면 호출됨
    public override void OnConnectedToMaster()
    {
        Debug.Log("클라이언트가 마스터에 연결됨");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("클라이언트가 로비에 연결됨");
        LoadingView.SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        GameObject tmpRoomButton;
        foreach (RoomInfo room in roomList.Where(r => r.PlayerCount < 2))
        {
            if (room.RemovedFromList == true)
            {
                localRoomDict.TryGetValue(room.Name, out tmpRoomButton);
                Destroy(tmpRoomButton);
                localRoomDict.Remove(room.Name);
            }
            else
            {
                if (!localRoomDict.ContainsKey(room.Name))
                {
                    tmpRoomButton = Instantiate(RoomButtonPrefab, roomButtonParentTransform);
                    tmpRoomButton.GetComponent<RoomButton>().roomName = room.Name;
                    tmpRoomButton.GetComponent<Button>().onClick.AddListener(() => { SetCurRoom(room.Name); roomSetCorrectly = true; });
                    localRoomDict.Add(room.Name, tmpRoomButton);
                }
                else
                {
                    localRoomDict.TryGetValue(room.Name, out tmpRoomButton);
                    tmpRoomButton.GetComponent<RoomButton>().roomName = room.Name;
                }
            }
        }
    }
    public void OnCreateRoomClicked()
    {
        if (string.IsNullOrEmpty(roomName.text) || localRoomDict.ContainsKey(roomName.text))
        {
            RoomNameAnnouncement();
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        isCreatingRoom = true;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);

        LoadingView.SetActive(true);
    }
    public void RoomNameAnnouncement()
    {
        roomnameAnounceText.gameObject.SetActive(true);
        Invoke("HideRoomNameAnnouncement", 1.5f);
    }
    public void HideRoomNameAnnouncement()
    {
        roomnameAnounceText.gameObject.SetActive(false);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("JoinedRoom");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            GameManager.instance.isHost = false;
        else
            GameManager.instance.isHost = true;

        PhotonNetwork.LoadLevel("LobbyScene");
    }
    public void SetCurRoom(string name)
    {
        CurrentRoomText.text = name;
    }
    public void JoinCurRoom()
    {
        if (roomSetCorrectly)
        {
            Debug.Log("JoinCurrnetRoom");
            PhotonNetwork.JoinRoom(CurrentRoomText.text);
        }
    }

    // 클라이언트가 어떤 방식으로든 연결이 끊어지면 호출됨
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("서버와의 연결이 끊어짐. 사유 : {0}", cause);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    bool gameStarted = false;
    public void StartGame()
    {
        if (gameStarted)
            return;
        else
            gameStarted = true;
        PhotonNetwork.LoadLevel("GameScene");
        Debug.Log("gamescene");
    }



}