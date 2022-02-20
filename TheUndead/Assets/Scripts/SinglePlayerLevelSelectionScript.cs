using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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



    // Start is called before the first frame update
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
        ShowScoreList(1643);
    }

    public void ShowScoreList(int id)
    {
        LootLockerSDKManager.GetScoreList(id, maxScore, (response) =>
        {
            if (response.success)
            {
                
                LootLockerLeaderboardMember[] scores = response.items;
                int values = scores.Length;
                if (scores.Length > maxScore) values = maxScore;
                for(int i = 0; i < values; i++)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(scores[i].score);
                    leaderboardTexts[i].text = scores[i].rank + " - " + ts.ToString("mm':'ss");
                }

                if(scores.Length < maxScore)
                {
                    int temp = maxScore - scores.Length;
                    for (int i = scores.Length; i < maxScore; i++)
                    {
                        leaderboardTexts[i].text = "n/a" + " - " + "00:00";
                    }
                }
            }
            else
            {

            }
        });
    }

    void BackButtonClick()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void Level1ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 1";
        uiImage.texture = level1Image;
        currentLevelSelected = "Level1";
        ShowScoreList(1643);
    }

    void Level2ButtonClick()
    {
        playButton.interactable = false;
        levelTitle.text = "Level 2";
        uiImage.texture = level2Image;
        currentLevelSelected = "Level2";
        ShowScoreList(1644);
    }

    void Level3ButtonClick()
    {
        playButton.interactable = false;
        levelTitle.text = "Level 3";
        uiImage.texture = level3Image;
        currentLevelSelected = "Level3";
        ShowScoreList(1645);
    }

    void Level4ButtonClick()
    {
        playButton.interactable = false;
        levelTitle.text = "Level 4";
        uiImage.texture = level4Image;
        currentLevelSelected = "Level4";
        ShowScoreList(1646);
    }

    void Level5ButtonClick()
    {
        playButton.interactable = false;
        levelTitle.text = "Level 5";
        uiImage.texture = level5Image;
        currentLevelSelected = "Level5";
        ShowScoreList(1647);
    }

    void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(currentLevelSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
