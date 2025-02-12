using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : MonoBehaviour
{
    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _joinRoom;
    [SerializeField] private TMP_InputField _roomName;

    void Start()
    {
        _createRoom.onClick.AddListener(CreateRoom);
        _joinRoom.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        _createRoom.onClick.RemoveListener(CreateRoom);
        _joinRoom.onClick.RemoveListener(JoinRoom);
    }

    public void CreateRoom()
    {
        MultiplayerController.Instance.CreateRoom(_roomName.text);
    }

    public void JoinRoom()
    {
        MultiplayerController.Instance.JoinRoom(_roomName.text);
    }
}
