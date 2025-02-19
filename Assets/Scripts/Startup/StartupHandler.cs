using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupHandler : MonoBehaviour
{
    [SerializeField] private GameObject _startupCanvasObj;
    [SerializeField] private GameObject _loadingCanvasObj;
    [SerializeField] private GameObject _failedToConnectText;

    void Start()
    {
        MultiplayerController.Instance._onConnectionResult = SendToAuthScreen;
    }

    private void Update()
    {
        if (!_startupCanvasObj.activeSelf) return;

        // Check if player press any key
        if (Input.anyKeyDown)
        {
            OnStartButtonClicked();
        }
    }

    private void OnStartButtonClicked()
    {
        _failedToConnectText.SetActive(false);
        // Hide the startup canvas
        _startupCanvasObj.SetActive(false);
        // Show the loading canvas
        _loadingCanvasObj.SetActive(true);
        // Initialize the multiplayer controller
        MultiplayerController.Instance.Initialize();
    }

    private void SendToAuthScreen(bool success)
    {
        if (success)
        {
            MultiplayerController.Instance._onConnectionResult = null;

            // Load the main menu scene
            SceneManager.LoadScene(SceneConstants.AuthScene);
        }
        else
        {
            // Show error message
            Debug.LogError("Failed to connect to server");
            SetFailedToConnectScreen();
        }
    }

    private void SetFailedToConnectScreen()
    {
        _loadingCanvasObj.SetActive(false);
        _startupCanvasObj.SetActive(true);
        _failedToConnectText.SetActive(true);
    }
}
