using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUIScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public bool isOnline = false;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button exitButton;

    void Start()
    {

        resumeButton.onClick.AddListener(Resume);
        if(!isOnline)
        {
            levelSelectionButton.onClick.AddListener(LevelSelectionButtonClick);
        } 
        else {
            levelSelectionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Main Menu";
            levelSelectionButton.onClick.AddListener(MainMenuButtonClick);
        }
        exitButton.onClick.AddListener(ExitButtonClick);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {

        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        if (!isOnline)
        {
            Time.timeScale = 1f;
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView view = player.GetComponent<PhotonView>();
                if (view.IsMine)
                {
                    player.GetComponentInChildren<PlayerRotationScript>().enabled = true;
                    player.GetComponentInChildren<PlayerMovementScript>().enabled = true;
                    player.GetComponentInChildren<GunScript>().enabled = true;
                }
            }
        }
        StartCoroutine(DisableCursor());
        
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        if (!isOnline)
        {
            Time.timeScale = 0f;
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject player in players)
            {
                PhotonView view = player.GetComponent<PhotonView>();
                if(view.IsMine)
                {
                    player.GetComponentInChildren<PlayerRotationScript>().enabled = false;
                    player.GetComponent<PlayerMovementScript>().enabled = false;
                    player.GetComponentInChildren<GunScript>().enabled = false;
                }
            }
        }
        StartCoroutine(EnableCursor());
    }

    private IEnumerator DisableCursor()
    {
        yield return new WaitForEndOfFrame();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator EnableCursor()
    {
        yield return new WaitForEndOfFrame();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void LevelSelectionButtonClick()
    {
        Resume();
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }

    void MainMenuButtonClick()
    {
        Resume();
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu");

    }

    void ExitButtonClick()
    {
        Application.Quit();
    }
}
