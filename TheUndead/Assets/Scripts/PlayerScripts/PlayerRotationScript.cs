using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationScript : MonoBehaviour
{

    public float playerMouseSensitivity = 100f;
    public Transform playerBody;
    public Transform gunBody;
    public GameObject player;
    private PhotonView view;
    public bool isSinglePlayerOverride = false;


    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        view = player.GetComponent<PhotonView>();
    }

    // Update is called once per frame
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
