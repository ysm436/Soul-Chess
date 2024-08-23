using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    TextMeshProUGUI buttonText;
    NetworkMasterManager networkMasterManager;
    public string roomName
    {
        set
        {
            _roomName = value;
            buttonText.text = _roomName;
        }
    }
    private string _roomName;
    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        networkMasterManager = GameObject.Find("NetworkMasterManager").GetComponent<NetworkMasterManager>();
    }
}
