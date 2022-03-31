using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerLevelSelection : MonoBehaviourPunCallbacks
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
        currentLevelSelected = "MPLevel1";
    }


    void BackButtonClick()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu");
    }


    void Level1ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 1";
        uiImage.texture = level1Image;
        currentLevelSelected = "MPLevel1";

    }

    void Level2ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 2";
        uiImage.texture = level2Image;
        currentLevelSelected = "MPLevel2";

    }

    void Level3ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 3";
        uiImage.texture = level3Image;
        currentLevelSelected = "MPLevel3";
    }

    void Level4ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 4";
        uiImage.texture = level4Image;
        currentLevelSelected = "MPLevel4";
    }

    void Level5ButtonClick()
    {
        playButton.interactable = true;
        levelTitle.text = "Level 5";
        uiImage.texture = level5Image;
        currentLevelSelected = "MPLevel5";
    }

    void OnPlayClick()
    {
        PlayerPrefs.SetString("MultiplayerLevel", currentLevelSelected);
        SceneManager.LoadSceneAsync("LobbyScene");
    }

}
