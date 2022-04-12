using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Photon.Realtime;

/// <summary>
/// Enum Class to show the player states
/// </summary>
enum PlayerState
{
    idle,
    run,
    walk,
    in_air,
}

/// <summary>
/// Class handles the Player Keyboard Input - Movement and Saving. 
/// </summary>
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


    private PhotonView view;

    public Text UIMessage;


    void Start()
    {
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

    /// <summary>
    /// Handles the player Movement. 
    /// </summary>
    void PlayerMovementHandler()
    {
        isSaveHit();


        if (isGrounded() && playerVelocity.y < 0)
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

    /// <summary>
    /// If Saving related keys are pressed they are handled here (saving + loading). 
    /// </summary>
    void isSaveHit()
    {
        if(Input.GetKeyDown(KeyCode.Z)) {
            String message = SavingScripts.Instance.SaveLevel();
            StartCoroutine(ShowUIMessage(message));
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            String message = SavingScripts.Instance.LoadSaveFile();
            StartCoroutine(ShowUIMessage(message));
        }
    }

    /// <summary>
    /// Show UI Message on scene for saving status. 
    /// </summary>
    IEnumerator ShowUIMessage(string message)
    {
        UIMessage.text = message;
        UIMessage.enabled = true;
        yield return new WaitForSeconds(2);
        UIMessage.enabled = false;
    }

    /// <summary>
    /// Jump handler for the player. 
    /// </summary>
    void PlayerJumpHandler()
    {
        if (Input.GetKeyDown("space") && isGrounded())
        {
            playerVelocity.y = Mathf.Sqrt((jumpHeight * playerJumpMultipler) * -2f * worldGravity);
            state = PlayerState.in_air;
            hasJumped = true;
        }
    }

    /// <summary>
    /// Check if the player is grounded on the terrain. 
    /// </summary>
    bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    /// <summary>
    /// Handles players sprinting. 
    /// </summary>
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

    /// <summary>
    /// return bool value on is player is moving. 
    /// </summary>
    bool PlayerIsInputting()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sound Handler to make it change depending on Walking vs Running. 
    /// </summary>
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

    /// <summary>
    /// In Game Menu Handler. 
    /// </summary>
    void inGameMenu()
    {
        if (Input.GetKeyDown("escape")) Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Load state from save player for Player Movement. 
    /// </summary>
    public void LoadSaveFile(PlayerSaveInformation saveInformation)
    {
        PlayerStatsScript playerStatsScript = GetComponent<PlayerStatsScript>();

        playerStatsScript.currentHealth = saveInformation.currentHealth;
        playerStatsScript.maxHealth = saveInformation.maxHealth;
        playerStatsScript.updateHealthUI();
        playerStatsScript.playerScore = saveInformation.playerScore;
        playerStatsScript.updateScoreUI();

        var delta = saveInformation.position - transform.position;
        controller.Move(delta);

        transform.rotation = saveInformation.rotation;

        GunScript gunScript = GetComponentInChildren<GunScript>();
        gunScript.loadSaveFile(saveInformation.ammo, saveInformation.ammoCapacity);

    }

}
