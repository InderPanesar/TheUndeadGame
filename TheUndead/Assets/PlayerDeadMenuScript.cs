using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeadMenuScript : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button returnToLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        retryButton.onClick.AddListener(RetryButtonClicked);
        returnToLevelButton.onClick.AddListener(ReturnToLevelClicked);
    }

    void RetryButtonClicked()
    {
        string level = PlayerPrefs.GetString("currentLevel");
        SceneManager.LoadSceneAsync(level);
    }

    void ReturnToLevelClicked()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }
}
