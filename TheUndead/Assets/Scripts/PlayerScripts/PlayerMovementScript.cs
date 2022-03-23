using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Photon.Realtime;

enum PlayerState
{
    idle,
    run,
    walk,
    in_air,
}

public class PlayerMovementScript : MonoBehaviourPunCallbacks
{
    public CharacterController controller;
    public float playerSpeed = 12f;
    public float jumpHeight = 4;
    public float worldGravity = -19;
    public AudioSource walkingAudioSource;
    public AudioSource runningAudioSource;
    public Camera camera;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isSinglePlayerOverride = false;


    Vector3 playerVelocity;
    float playerSpeedMultipler = 1f;
    float playerJumpMultipler = 1f;

    PlayerState state = PlayerState.idle;
    bool hasJumped = false;

    bool setupScores = false;

    private PhotonView view;
    private Text scoreText;

    private int playerScore = 0;

    public void UpdateScore()
    {
        if(isSinglePlayerOverride)
        {
            increaseScore();
        }
        else
        {
            view.RPC("increaseScore", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void increaseScore()
    {
        this.playerScore++;
        if(scoreText == null) scoreText = (Text)GameObject.FindWithTag("Player Score Text HUD").GetComponent<Text>() as Text;

        scoreText.text = "Player Score: " + playerScore;
        if (playerScore == 6)
        {
            CompleteLevel();
        }
    }


    private void CompleteLevel()
    {
        if(isSinglePlayerOverride)
        {
            //ToDo: Update with new string when implemented.
            PlayerPrefs.SetFloat("levelCompleteTime", Time.timeSinceLevelLoad);
            SubmitScoreToLeaderboard(Time.timeSinceLevelLoad);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync("GameWinScene");
        }
        else
        {
            SceneManager.LoadSceneAsync("MultiplayerEndingLevel");

        }



    }


    public void SubmitScoreToLeaderboard(float time)
    {
        String level = PlayerPrefs.GetString("currentLevel", "");
        if(level != "")
        {
            StartCoroutine(LeaderboardScript.Instance.AddScore(time, level));
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
        if(view == null && isSinglePlayerOverride)
        {
            camera.enabled = true;
        }
        else
        {
            if (view.IsMine)
            {
                camera.enabled = true;
            }
        }
    }

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

    void Update()
    {
        if (view == null && isSinglePlayerOverride)
        {
            PlayerMovementHandler();
        }
        else
        {
            if (view.IsMine)
            {
                PlayerMovementHandler();
            }
        }


    }
    
    void PlayerMovementHandler()
    {

        if(isGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        if(!isGrounded())
        {
            state = PlayerState.in_air;
        }
        else
        {
            state = PlayerState.idle;
        }


        float horizontalValue = Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalValue = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 move = transform.right * horizontalValue + transform.forward * verticalValue;
        //Stops diagonally fast movement
        if(move.magnitude > 1) move /= move.magnitude;
        PlayerSprintHandler();
        if (move.magnitude > 0) PlayerSoundHandler();
        controller.Move(move * (playerSpeed * playerSpeedMultipler));

        //Player Jump handler 
        PlayerJumpHandler();

        //Add Gravity to the player
        playerVelocity.y += worldGravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        inGameMenu();
    }

    void PlayerJumpHandler()
    {
        if (Input.GetKeyDown("space") && isGrounded())
        {
            playerVelocity.y = Mathf.Sqrt((jumpHeight * playerJumpMultipler) * -2f * worldGravity);
            state = PlayerState.in_air;
            hasJumped = true;
        }
    }

    bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void PlayerSprintHandler()
    {
        if (Input.GetButton("Sprint") && isGrounded())
        {
            playerSpeedMultipler = 2f;
            playerJumpMultipler = 1.5f;
            if (walkingAudioSource.isPlaying) walkingAudioSource.Pause();
        }
        else
        {
            playerSpeedMultipler = 1f;
            playerJumpMultipler = 1f;
            if (runningAudioSource.isPlaying) runningAudioSource.Pause();
        }

        if (PlayerIsInputting())
        {
            state = PlayerState.walk;
            if (playerSpeedMultipler == 2f)
            {
                state = PlayerState.run;
            }
        }
    }

    bool PlayerIsInputting()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            return true;
        }
        return false;
    }

    void PlayerSoundHandler()
    {
        if(isGrounded() && PlayerIsInputting() && !walkingAudioSource.isPlaying && !runningAudioSource.isPlaying && playerSpeedMultipler == 2f)
        {
            runningAudioSource.Play();
            runningAudioSource.volume = UnityEngine.Random.Range(0.8f, 1.0f);
            runningAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
        }
        else if(isGrounded() && PlayerIsInputting() && !walkingAudioSource.isPlaying && !runningAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
            walkingAudioSource.volume = UnityEngine.Random.Range(0.8f, 1.0f);
            walkingAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
        }
    }

    void inGameMenu()
    {
        if (Input.GetKeyDown("escape")) Cursor.lockState = CursorLockMode.None;
    }


}
