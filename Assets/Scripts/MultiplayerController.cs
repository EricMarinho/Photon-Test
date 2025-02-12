using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    //Singleton
    public static MultiplayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        ConnectToServer();
        JoinLobby();
        //StartCoroutine(CheckPing());
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private IEnumerator CheckPing()
    {
        while (true)
        {
            Debug.Log("Ping: " + PhotonNetwork.GetPing());
            yield return new WaitForSeconds(5);
        }
    }

    #region Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }

    #endregion
}