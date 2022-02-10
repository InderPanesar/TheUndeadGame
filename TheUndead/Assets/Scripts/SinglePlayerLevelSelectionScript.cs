using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        //Make sure cursor is visible and not locked.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        level1Button.onClick.AddListener(Level1ButtonClick);
        backButton.onClick.AddListener(BackButtonClick);
    }

    void BackButtonClick()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void Level1ButtonClick()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
