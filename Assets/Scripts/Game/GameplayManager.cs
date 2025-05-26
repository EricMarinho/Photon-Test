using Photon.Pun;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (player != null)
        {
            // Instantiate the player at a specific position and rotation
            Vector3 spawnPosition = new Vector3(0, 1, 0); // Example position
            Quaternion spawnRotation = Quaternion.identity; // Default rotation
            PhotonNetwork.Instantiate(player.name, spawnPosition, spawnRotation, 0);
        }
        else
        {
            Debug.LogError("Player prefab is not assigned in the GameplayManager.");
        }
    }
}
