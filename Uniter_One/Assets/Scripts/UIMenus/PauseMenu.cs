using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    private static bool gameInPaused;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameInPaused)
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
        gameInPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        gameInPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}