using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to handle the mouse rotation for camera.
/// </summary>
public class PlayerRotationScript : MonoBehaviour
{

    public float playerMouseSensitivity = 100f;
    public Transform playerBody;
    public Transform gunBody;
    public GameObject player;
    private PhotonView view;
    public bool isSinglePlayerOverride = false;


    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        view = player.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view == null && isSinglePlayerOverride)
        {
            PlayerRotationHandler();
        }
        else
        {
            if (view.IsMine)
            {
                PlayerRotationHandler();
            }
        }
    }

    /// <summary>
    /// Handles the rotation of the camera around the screen.
    /// </summary>
    void PlayerRotationHandler()
    {
        float mouseXValue = Input.GetAxis("Mouse X") * playerMouseSensitivity * Time.deltaTime;
        float mouseYValue = Input.GetAxis("Mouse Y") * playerMouseSensitivity * Time.deltaTime;

        xRotation -= mouseYValue;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseXValue);
    }
}
