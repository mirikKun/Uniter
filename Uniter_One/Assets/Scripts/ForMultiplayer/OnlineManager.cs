﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineManager : MonoBehaviourPunCallbacks
{
  public Spawner spawner;
  public GameObject player;

  private void Start()
  {
    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager") , Vector3.zero, Quaternion.identity);
  }

  public void Leave()
  {
    PhotonNetwork.LeaveRoom();
  }

  public override void OnPlayerLeftRoom(Player otherPlayer)
  {
    Debug.LogFormat("Player {0} entered room",otherPlayer.NickName);
  }

  public override void OnPlayerEnteredRoom(Player newPlayer)
  {
    Debug.LogFormat("Player {0} left room",newPlayer.NickName);
  }

  public override void OnLeftRoom()
  {
    SceneManager.LoadScene(0);
  }
}
