using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for player dead scene.
/// </summary>
public class PlayerDeadMenuScript : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button returnToLevelButton;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        retryButton.onClick.AddListener(RetryButtonClicked);
        returnToLevelButton.onClick.AddListener(ReturnToLevelClicked);
    }

    /// <summary>
    /// Called when retry button is clicked on scene.
    /// </summary>
    void RetryButtonClicked()
    {
        string level = PlayerPrefs.GetString("currentLevel");
        SceneManager.LoadSceneAsync(level);
    }

    /// <summary>
    /// Called when level selection button is clicked on scene.
    /// </summary>
    void ReturnToLevelClicked()
    {
        SceneManager.LoadSceneAsync("SinglePlayerLevelSelection");
    }
}
