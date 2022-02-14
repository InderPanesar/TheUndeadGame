using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    idle,
    run,
    walk,
    in_air,
}

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 12f;
    public float jumpHeight = 4;
    public float worldGravity = -19;
    public AudioSource walkingAudioSource;
    public AudioSource runningAudioSource;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 playerVelocity;
    float playerSpeedMultipler = 1f;
    float playerJumpMultipler = 1f;

    PlayerState state = PlayerState.idle;
    bool hasJumped = false;

    bool setupScores = false;


    void Start()
    {
        if(setupScores == false)
        {
            SetupScores();
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
        PlayerMovementHandler();
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
            runningAudioSource.volume = Random.Range(0.8f, 1.0f);
            runningAudioSource.pitch = Random.Range(0.8f, 1.0f);
        }
        else if(isGrounded() && PlayerIsInputting() && !walkingAudioSource.isPlaying && !runningAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
            walkingAudioSource.volume = Random.Range(0.8f, 1.0f);
            walkingAudioSource.pitch = Random.Range(0.8f, 1.0f);
        }
    }

    void inGameMenu()
    {
        if (Input.GetKeyDown("escape")) Cursor.lockState = CursorLockMode.None;
    }



}
