using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script to handle when the game has been won.
/// </summary>
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
        SubmitScoreToLeaderboard(timeToComplete);
        TimeSpan ts = TimeSpan.FromSeconds(timeToComplete);
        timeTakenToComplete.text = "Time Taken to Complete: " + ts.ToString("mm':'ss");

        retryButton.onClick.AddListener(RetryButtonClicked);
        retryButton.interactable = false;

        doneButton.onClick.AddListener(DoneButtonClicked);
        doneButton.interactable = false;


    }

    /// <summary>
    /// Method when retry level button is clicked.
    /// </summary>
    void RetryButtonClicked()
    {
        string level = PlayerPrefs.GetString("currentLevel");
        SceneManager.LoadSceneAsync(level);
    }

    /// <summary>
    /// Method when done with level button is clicked.
    /// </summary>
    void DoneButtonClicked()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }



    /// <summary>
    /// Submit the time to the leaderboard after completion of level.
    /// </summary>
    public void SubmitScoreToLeaderboard(float time)
    {
        string level = PlayerPrefs.GetString("currentLevel", "");
        if (level != "")
        {
            StartCoroutine(LeaderboardScript.Instance.AddScore(time, level, scoreAddedCallback));
        }

    }

    /// <summary>
    /// Allow user to continue to next page once leaderboard has handled request.
    /// </summary>
    public void scoreAddedCallback(bool done)
    {
        retryButton.interactable = true;
        doneButton.interactable = true;
    }
}
