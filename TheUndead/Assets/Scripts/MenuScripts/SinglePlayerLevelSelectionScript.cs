using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for a the single player selection.
/// </summary>
public class SinglePlayerLevelSelectionScript : MonoBehaviour
{

    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button level4Button;
    [SerializeField] private Button level5Button;
    [SerializeField] private Button backButton;

    [SerializeField] private Texture level1Image;
    [SerializeField] private Texture level2Image;
    [SerializeField] private Texture level3Image;
    [SerializeField] private Texture level4Image;
    [SerializeField] private Texture level5Image;


    [SerializeField] private TextMeshProUGUI[] leaderboardTexts;
    private int maxScore = 5;

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private RawImage uiImage;

    private String currentLevelSelected = "";

    void Start()
    {
        //Make sure cursor is visible and not locked.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        level1Button.onClick.AddListener(Level1ButtonClick);
        level2Button.onClick.AddListener(Level2ButtonClick);
        level3Button.onClick.AddListener(Level3ButtonClick);
        level4Button.onClick.AddListener(Level4ButtonClick);
        level5Button.onClick.AddListener(Level5ButtonClick);
        playButton.onClick.AddListener(OnPlayClick);


        backButton.onClick.AddListener(BackButtonClick);

        playButton.interactable = true;
        levelTitle.text = "Level 1";
        uiImage.texture = level1Image;
        currentLevelSelected = "Level1";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Show all the scores for a specific levels.
    /// </summary>
    private void ShowScoreList(String tableName)
    {
        StartCoroutine(LeaderboardScript.Instance.GetHighScores(tableName, maxScore, DisplayLeaderboard));
    }

    /// <summary>
    /// Displays the results of a leaderboard results on the UI.
    /// </summary>
    private void DisplayLeaderboard(List<LeaderboardResult> scores)
    {
        int values = scores.Count;
        if (scores.Count > maxScore) values = maxScore;

        for (int i = 0; i < values; i++)
        {
            TimeSpan ts = TimeSpan.FromSeconds(scores[i].score);
            leaderboardTexts[i].text = (i + 1) + " - " + ts.ToString("mm':'ss");
        }

        if (scores.Count < maxScore)
        {
            int temp = maxScore - scores.Count;
            for (int i = scores.Count; i < maxScore; i++)
            {
                leaderboardTexts[i].text = "n/a" + " - " + "00:00";
            }
        }
    }

    /// <summary>
    /// Handles when back button is pressed on UI.
    /// </summary>
    void BackButtonClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    /// <summary>
    /// Handles when Level 1 button is pressed on UI.
    /// </summary>
    void Level1ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 1";
        uiImage.texture = level1Image;
        currentLevelSelected = "Level1";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Handles when Level 2 button is pressed on UI.
    /// </summary>
    void Level2ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 2";
        uiImage.texture = level2Image;
        currentLevelSelected = "Level2";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Handles when Level 3 button is pressed on UI.
    /// </summary>
    void Level3ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 3";
        uiImage.texture = level3Image;
        currentLevelSelected = "Level3";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Handles when Level 4 button is pressed on UI.
    /// </summary>
    void Level4ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 4";
        uiImage.texture = level4Image;
        currentLevelSelected = "Level4";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Handles when Level 5 button is pressed on UI.
    /// </summary>
    void Level5ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 5";
        uiImage.texture = level5Image;
        currentLevelSelected = "Level5";
        ShowScoreList(currentLevelSelected);
    }

    /// <summary>
    /// Handles when play is pressed on the UI.
    /// </summary>
    void OnPlayClick()
    {
        PlayerPrefs.SetString("currentLevel", currentLevelSelected);
        SceneManager.LoadSceneAsync(currentLevelSelected);
    }

}
