using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiPlayerButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        //Make sure cursor is visible and not locked.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        singlePlayerButton.onClick.AddListener(SinglePlayerButtonClick);
        multiPlayerButton.onClick.AddListener(MultiPlayerButtonClick);
        settingsButton.onClick.AddListener(SettingsButtonClick);
        exitButton.onClick.AddListener(ExitButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SinglePlayerButtonClick()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }

    void MultiPlayerButtonClick()
    {
        SceneManager.LoadSceneAsync("LoadingScene");
    }

    void SettingsButtonClick()
    {
        SceneManager.LoadSceneAsync("SettingsPage");
    }

    void ExitButtonClick()
    {
        Application.Quit();
    }

}



