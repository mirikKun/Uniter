using System;
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
    private PhotonView PV;
    public Perlin3D perlin;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("AskForRoomSetting", RpcTarget.Others, PhotonNetwork.NickName);
        if (!FillOptions.join)
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero,
                Quaternion.identity);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    private void AskForRoomSetting(String nickname)
    {
        PV.RPC("GetSetting", RpcTarget.Others, nickname, FillOptions.size, FillOptions.enemyCount, FillOptions.useWalls,
            FillOptions.outerLightEnable,FillOptions.randomRoomGeneration,FillOptions.offset,FillOptions.bounce,FillOptions.multiplyIn,FillOptions.defaultRoom);
    }

    [PunRPC]
    private void GetSetting(String nickname, int size, int enemyCount, bool useWalls, bool outerLight,bool random,int offset,float bounce,float multiplyIn,bool defaultRoom)
    {
        if (PhotonNetwork.NickName != nickname)
            return;
        FillOptions.size = size;
        FillOptions.enemyCount = enemyCount;
        FillOptions.useWalls = useWalls;
        FillOptions.outerLightEnable = outerLight;
        FillOptions.randomRoomGeneration = random;
        FillOptions.offset = offset;
        FillOptions.bounce = bounce;
        FillOptions.multiplyIn = multiplyIn;
        FillOptions.defaultRoom = defaultRoom;
        perlin.StartMakingRoom();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("Player {0} entered room", otherPlayer.NickName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Player {0} left room", newPlayer.NickName);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}