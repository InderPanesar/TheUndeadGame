using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinScript : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button doneButton;
    [SerializeField] private TextMeshProUGUI timeTakenToComplete;

    // Start is called before the first frame update
    void Start()
    {
        //Make sure cursor is visible and not locked.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float timeToComplete = PlayerPrefs.GetFloat("levelCompleteTime");
        TimeSpan ts = TimeSpan.FromSeconds(timeToComplete);
        timeTakenToComplete.text = "Time Taken to Complete: " + ts.ToString("mm':'ss");

        retryButton.onClick.AddListener(RetryButtonClicked);
        doneButton.onClick.AddListener(DoneButtonClicked);

    }

    void RetryButtonClicked()
    {
        String level = PlayerPrefs.GetString("currentLevel");
        SceneManager.LoadSceneAsync(level);
    }

    void DoneButtonClicked()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }
}
