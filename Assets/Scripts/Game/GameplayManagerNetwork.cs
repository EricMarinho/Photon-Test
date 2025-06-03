using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManagerNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] private string playerPrefabName = "Player";
    [SerializeField] private float countdownTime = 3f;

    [SerializeField] private TMP_Text UIText;

    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private Button returnToMenu;

    private bool gameEnded = false;

    private void Start()
    {
        StartCoroutine(WaitForPhotonAndSpawn());
        returnToMenu.onClick.AddListener(ReturnToMenu);
    }

    private IEnumerator WaitForPhotonAndSpawn()
    {
        // Wait until the player has joined the room
        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }

        // Wait until at least 2 players are in the room
        while (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            UIText.text = "Aguardando outro jogador...";
            yield return null;
        }

        // Spawn the local player
        SpawnPlayer();

        // Only the MasterClient starts the countdown
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartCountdownRPC", RpcTarget.All, countdownTime);
        }
    }

    [PunRPC]
    private void StartCountdownRPC(float time)
    {
        StartCoroutine(StartGameAfterCountdown(time));
    }

    private IEnumerator StartGameAfterCountdown(float countdownTime)
    {
        UIText.text = $"Jogo começando em {countdownTime} segundos...";
        yield return new WaitForSeconds(countdownTime);
        UIText.text = "Jogo iniciado! Chegue primeiro à linha de chegada.";
        StartGame();
    }

    private void StartGame()
    {
        // Search for the PlayerController belonging to the local player
        foreach (var controller in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.InstanceID))
        {
            // Check ownership
            PhotonView view = controller.GetComponent<PhotonView>();
            if (view != null && view.IsMine)
            {
                controller.enabled = true;
                Debug.Log("PlayerController initialized.");
                break;
            }
        }
    }

    public void SpawnPlayer()
    {
        if (string.IsNullOrEmpty(playerPrefabName))
        {
            Debug.LogError("Player prefab name is not set.");
            return;
        }

        Vector3 spawnPosition = new Vector3(0, 1, 0); // Can be randomized or use spawn points
        Quaternion spawnRotation = Quaternion.identity;

        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, spawnRotation);

        // If it is the owner, attach the camera
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (photonView != null && photonView.IsMine)
        {
            // Create a new camera object
            GameObject cameraObject = Camera.main.gameObject;

            cameraObject.transform.SetParent(player.transform);

            Vector3 cameraOffset = new Vector3(0, 2, -4);
            cameraObject.transform.localPosition = cameraOffset;

            cameraObject.transform.LookAt(player.transform.position + Vector3.up * 1.5f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        PhotonView view = other.GetComponent<PhotonView>();
        if (view != null && view.IsMine)
        {
            gameEnded = true;

            string winnerName = PhotonNetwork.NickName;

            // Chama o RPC para todos mostrarem o vencedor
            photonView.RPC("EndGame", RpcTarget.All, winnerName);
        }
    }

    [PunRPC]
    private void EndGame(string winnerName)
    {
        if (endGameScreen != null)
            endGameScreen.SetActive(true);

        if (UIText != null)
            UIText.text = $"{winnerName} venceu a corrida!";

        foreach (var controller in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.InstanceID))
        {
            PhotonView view = controller.GetComponent<PhotonView>();
            if (view != null && view.IsMine)
            {
                controller.enabled = false;
                break;
            }
        }
    }

    public void ReturnToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }
}
