using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUIScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button exitButton;

    void Start()
    {

        resumeButton.onClick.AddListener(Resume);
        levelSelectionButton.onClick.AddListener(LevelSelectionButtonClick);
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
        Time.timeScale = 1f;
        GameIsPaused = false;
        StartCoroutine(DisableCursor());
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
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

    void ExitButtonClick()
    {
        Application.Quit();
    }
}
