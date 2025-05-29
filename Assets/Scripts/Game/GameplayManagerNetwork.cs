using Photon.Pun;
using UnityEngine;
using System.Collections;

public class GameplayManagerNetwork : MonoBehaviour
{
    [SerializeField] private string playerPrefabName = "Player";

    private void Start()
    {
        StartCoroutine(WaitForPhotonAndSpawn());
    }

    private IEnumerator WaitForPhotonAndSpawn()
    {
        // Espera até que o jogador tenha realmente entrado na sala
        while (!PhotonNetwork.InRoom)
        {
            yield return null; // espera o próximo frame
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (string.IsNullOrEmpty(playerPrefabName))
        {
            Debug.LogError("Player prefab name is not set.");
            return;
        }

        Vector3 spawnPosition = new Vector3(0, 1, 0); // Pode ser randomizado ou com spawn points
        Quaternion spawnRotation = Quaternion.identity;

        PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, spawnRotation);
    }
}