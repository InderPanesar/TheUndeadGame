using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for main menu page.
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiPlayerButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    void Start()
    {
        //Make sure cursor is visible and not locked.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        singlePlayerButton.onClick.AddListener(SinglePlayerButtonClick);
        multiPlayerButton.onClick.AddListener(MultiPlayerButtonClick);
        settingsButton.onClick.AddListener(SettingsButtonClick);
        exitButton.onClick.AddListener(ExitButtonClick);

        AudioListener.volume = PlayerPrefs.GetFloat("AudioVolume", 1);
    }

    void Update()
    {

    }

    /// <summary>
    /// Method when Single Player Button is clicked.
    /// </summary>
    void SinglePlayerButtonClick()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }

    /// <summary>
    /// Method when Multi-player button is clicked.
    /// </summary>
    void MultiPlayerButtonClick()
    {
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    /// <summary>
    /// Method when Settings button is clicked.
    /// </summary>
    void SettingsButtonClick()
    {
        SceneManager.LoadSceneAsync("SettingsPage");
    }

    /// <summary>
    /// Method when exit button is clicked.
    /// </summary>
    void ExitButtonClick()
    {
        Application.Quit();
    }

}



