using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthController : MonoBehaviour
{
    private string _playerName;
    [SerializeField] private TMP_InputField _playerNameInputField;
    [SerializeField] private Button _createUserButton;
    [SerializeField] private GameObject _authScreen;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _failedToConnectText;

    void Start()
    {
        MultiplayerController.Instance._onConnectionResult = SendToMainMenu;

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.Nickname))
        {
            _authScreen.SetActive(false);
            _loadingScreen.SetActive(true);

            _playerName = PlayerPrefs.GetString(PlayerPrefsConstants.Nickname);
            MultiplayerController.Instance.ChangeNickname(_playerName);
            MultiplayerController.Instance.JoinLobby();
            return;
        }

        _playerName = "Player_" + Random.Range(1, 9999);
        _createUserButton.onClick.AddListener(CreateUser);
        _createUserButton.interactable = false;

        _playerNameInputField.onValueChanged.AddListener(OnPlayerNameChanged);
    }

    private void OnDestroy()
    {
        _createUserButton.onClick.RemoveListener(CreateUser);
        _playerNameInputField.onValueChanged.RemoveListener(OnPlayerNameChanged);
    }

    private void CreateUser()
    {
        _playerName = _playerNameInputField.text;
        
        PlayerPrefs.SetString(PlayerPrefsConstants.Nickname, _playerName);
        PlayerPrefs.Save();

        _authScreen.SetActive(false);
        _loadingScreen.SetActive(true);
        MultiplayerController.Instance.ChangeNickname(_playerName);
        MultiplayerController.Instance.JoinLobby();
    }

    private void OnPlayerNameChanged(string value)
    {
        _createUserButton.interactable = !string.IsNullOrEmpty(value);
    }

    private void SendToMainMenu(bool isSucces)
    {
        if(isSucces)
        {
            MultiplayerController.Instance._onConnectionResult = null;

            // Load the main menu scene
            SceneManager.LoadScene(SceneConstants.MainMenu);
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
        _failedToConnectText.SetActive(true);
        _loadingScreen.SetActive(false);

        StartCoroutine(ReturnToStartupScene());
    }

    private System.Collections.IEnumerator ReturnToStartupScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneConstants.StartupScene);
    }
}
