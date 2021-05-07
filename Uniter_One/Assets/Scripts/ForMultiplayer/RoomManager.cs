using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private static RoomManager MyRoomManager;

    private void Awake()
    {
        if (MyRoomManager)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        MyRoomManager = this;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Arena")
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero,
                Quaternion.identity);
        }
    }
}