using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script to handle player stats such as Health and Score.
/// </summary>
public class PlayerStatsScript : MonoBehaviour
{

    private PhotonView view;
    private Text scoreText;
    private Image playerHealthBar;

    public bool isSinglePlayerOverride = false;
    bool setupScores = false;

    public int playerScore = 0;
    public float maxHealth = 100;
    public float currentHealth = 100;


    /// <summary>
    /// Check if the player has killed all enemies in the scene.
    /// </summary>
    private void CheckIfScoreLimitMet()
    {

        GameObject[] enemies;
        if (isSinglePlayerOverride)
        {
            bool allAreDead = true;

            GameObject enemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer");
            EnemiesContainerScript containerScript = enemyContainer.GetComponent<EnemiesContainerScript>();
            enemies = containerScript.enemies;

            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject enemy = enemies[i];
                RaycastTargetScript targetScript = enemy.GetComponent<RaycastTargetScript>();
                if (targetScript.isEnabled)
                {
                    allAreDead = false;
                    break;
                }
            }

            if (allAreDead)
            {
                CompleteLevel();
            }
        }
        else
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemies");

            if (enemies.Length == 0)
            {
                view.RPC("CompleteLevel", RpcTarget.AllBuffered);
            }
        }


    }

    /// <summary>
    /// Update the UI appropriately to show Players Score - Single Player.
    /// </summary>
    public void updateScoreUI()
    {
        if (scoreText == null) scoreText = (Text)GameObject.FindWithTag("Player Score Text HUD").GetComponent<Text>() as Text;
        scoreText.text = "Player Score: " + playerScore;
    }

    /// <summary>
    /// Update the UI appropriately to show Players Health - Single Player.
    /// </summary>
    public void updateHealthUI()
    {
        if (playerHealthBar == null) playerHealthBar = (Image)GameObject.FindWithTag("UI Health Bar").GetComponent<Image>() as Image;

        float values = (currentHealth / maxHealth);
        playerHealthBar.fillAmount = values;
    }

    /// <summary>
    /// Allows the player to take damage.
    /// </summary>
    public void TakeDamage()
    {

        if (isSinglePlayerOverride)
        {
            updateHealthBar();
        }
        else
        {
            //Multiplayer both players have the same health bar.
            //So damage done to one must be synced to all other players.
            //Hence AllBuffered is used just incase a new player was to join the lobby.
            view.RPC("updateHealthBar", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Update the UI appropriately to show Players Health - Multiplayer.
    /// </summary>
    [PunRPC]
    private void updateHealthBar()
    {
        if (playerHealthBar == null) playerHealthBar = (Image)GameObject.FindWithTag("UI Health Bar").GetComponent<Image>() as Image;


        currentHealth -= 10;



        float values = (currentHealth / maxHealth);

        playerHealthBar.fillAmount = values;


        if (currentHealth <= 0)
        {
            if (isSinglePlayerOverride)
            {
                SceneManager.LoadSceneAsync("GameLostScene");

            }
            else
            {
                SceneManager.LoadSceneAsync("MultiplayerLevelYouDied");
            }
        }
    }

    /// <summary>
    /// Update the UI appropriately to show Players Score - Multiplayer.
    /// </summary>
    public void UpdateScore()
    {
        if (isSinglePlayerOverride)
        {
            increaseScore();
        }
        else
        {
            view.RPC("increaseScore", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Increases the score for the player.
    /// </summary>
    [PunRPC]
    private void increaseScore()
    {
        this.playerScore++;
        if (scoreText == null) scoreText = (Text)GameObject.FindWithTag("Player Score Text HUD").GetComponent<Text>() as Text;

        scoreText.text = "Player Score: " + playerScore;



        if (isSinglePlayerOverride)
        {
            CheckIfScoreLimitMet();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CheckIfScoreLimitMet();
            }
        }
    }


    /// <summary>
    /// Complete the level.
    /// </summary>
    [PunRPC]
    private void CompleteLevel()
    {
        if (isSinglePlayerOverride)
        {
            //ToDo: Update with new string when implemented.
            PlayerPrefs.SetFloat("levelCompleteTime", Time.timeSinceLevelLoad);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync("GameWinScene");
        }
        else
        {
            SceneManager.LoadSceneAsync("MultiplayerEndingLevel");
        }



    }

    void Start()
    {
        scoreText = (Text)GameObject.FindWithTag("Player Score Text HUD").GetComponent<Text>() as Text;

        if (setupScores == false)
        {
            SetupScores();
        }

        view = GetComponent<PhotonView>();

    }

    /// <summary>
    /// Setup the scores for each level in the PlayerPrefs for each level.
    /// </summary>
    void SetupScores()
    {
        PlayerPrefs.SetInt("level1score", 0);
        PlayerPrefs.SetInt("level2score", 0);
        PlayerPrefs.SetInt("level3score", 0);
        PlayerPrefs.SetInt("level4score", 0);
        PlayerPrefs.SetInt("level5score", 0);
        PlayerPrefs.Save();
        setupScores = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
