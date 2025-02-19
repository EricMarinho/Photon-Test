using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    //Singleton
    public static MultiplayerController Instance;
    public Action<bool> _onConnectionResult;

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
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void ChangeNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
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

    public void JoinOrCreateRandomRoom()
    {
        string roomName = "Room_" + UnityEngine.Random.Range(1, 9999);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }

    public IEnumerator CheckPing()
    {
        while (true)
        {
            Debug.Log("Ping: " + PhotonNetwork.GetPing());
            yield return new WaitForSeconds(5);
        }
    }

    private void LateInitialize()
    {
        ConnectToServer();
        JoinLobby();
    }

    #region Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Server");
    }

    public override void OnConnectedToMaster()
    {
        _onConnectionResult?.Invoke(true);
        Debug.Log("Connected to Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _onConnectionResult?.Invoke(false);
        Debug.Log("Disconnected from server for reason: " + cause.ToString());
    }

    public override void OnJoinedLobby()
    {
        _onConnectionResult?.Invoke(true);
        Debug.Log("Joined Lobby");
    }

    public override void OnLeftLobby()
    {
        _onConnectionResult.Invoke(false);
        Debug.Log("Left Lobby");
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