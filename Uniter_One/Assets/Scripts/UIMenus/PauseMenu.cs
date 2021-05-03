using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool GameInPaused ;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameInPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameInPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        GameInPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
